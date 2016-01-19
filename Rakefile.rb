require "./rake-tasks/init"

#-----------------------------------SETTINGS-----------------------------------

$binaries_baselocation = "bin"
$nuget_baselocation = "nugetArtifacts"
$app_version = "9.9.9.999"

#--------------------------------build settings--------------------------------

$build_configuration = "Debug"

msbuild_settings = {
  :properties => {:configuration => $build_configuration},
  :targets => [:clean, :rebuild],
  :verbosity => :minimal,
  #:use => :net35  ;uncomment to use .net 3.5 - default is 4.0
}

#-------------------------------project settings-------------------------------

$solution = "source/Habanero.sln"
$major_version = ""
$minor_version = ""
$patch_version = ""
$nuget_apikey = ""
$nuget_sourceurl = ""
$nuget_publish_version = "Trunk"

#______________________________________________________________________________
#-------------------------------------TASKS------------------------------------

desc "Runs the build task"
task :default, [:major, :minor, :patch] => [:setupvars, :build_test]

desc "Runs the build task and pushes to local"
task :build_push, [:major, :minor, :patch, :apikey, :sourceurl] => [:setupvars, :build_test, :nugetpush]

desc "Pushes NuGet packages to local"
task :build_push_nobuild, [:major, :minor, :patch, :apikey, :sourceurl] => [:setupvars, :nugetpush]

desc "Builds Habanero, including tests"
task :build_test => [:build_only, :test]

desc "Builds Habanero"
task :build_only, [:major, :minor, :patch, :apikey, :sourceurl] => [:update_packages, :clean, :set_assembly_version, :msbuild, :copy_to_nuget]

desc "Builds Habanero, including running tests with dotcover then pushes to the local nuget server"
task :build_with_coverage, [:major, :minor, :patch, :apikey, :sourceurl] => [:build_only, :test_with_coverage]

desc "Build with sonar (stats build)"
task :build_with_sonar, [:major, :minor, :patch, :apikey, :sourceurl] => [:build_with_coverage, :sonar]

#--------------------------------Update packages-------------------------------

desc "Updates packages with nuget"
updatenugetpackages :update_packages do |nuget|
    puts cyan("Updating nuget packages")
    nuget.solution = $solution
end

#-------------------------------Prepare For Build------------------------------

task :setupvars, :major, :minor, :patch, :apikey, :sourceurl do |t, args|
    puts cyan("Setup Variables")
    args.with_defaults(:major => "0")
    args.with_defaults(:minor => "0")
    args.with_defaults(:patch => "0000")
    args.with_defaults(:apikey => "")
    args.with_defaults(:sourceurl => "")
    $major_version = "#{args[:major]}"
    $minor_version = "#{args[:minor]}"
    $patch_version = "#{args[:patch]}"
    $nuget_apikey = "#{args[:apikey]}"
    $nuget_sourceurl = "#{args[:sourceurl]}"
    $app_version = "#{$major_version}.#{$minor_version}.#{$patch_version}.0"
    puts cyan("Assembly Version #{$app_version}")
    puts cyan("Nuget key: #{$nuget_apikey} for: #{$nuget_sourceurl}")
end

desc "Cleans the bin folder and all project bin folders"
task :clean do
    puts cyan("Cleaning all bin and test folders")
    clean_build_output_directories
    FileUtils.rm_rf $binaries_baselocation
    FileUtils.rm_rf $nuget_baselocation
end

def clean_build_output_directories
    bin_dirs = Dir.glob("**/bin").select {|f| (File.directory? f) && !(f.match(/\/node_modules\//) || f.match(/\/packages\//) || f.match(/\/obj\//) || f.match(/\/rake-tasks\//))}
    for project_output in bin_dirs
        FileUtils.rm_rf project_output
    end
end

task :set_assembly_version do
    puts green("Setting Shared AssemblyVersion to: #{$app_version}")
    file_path = "source/Common/AssemblyInfoShared.cs"
    outdata = File.open(file_path).read.gsub(/"9.9.9.999"/, "\"#{$app_version}\"")
    File.open(file_path, "w") do |out|
        out << outdata
    end
end

#--------------------------------Build Solution--------------------------------

desc "Builds the solution with msbuild"
msbuild :msbuild do |msb|
    puts cyan("Building #{$solution} with msbuild")
    msb.update_attributes msbuild_settings
    msb.solution = $solution
end

#-----------------------------------Run Tests----------------------------------

desc "Runs the tests"
nunit :test do |nunit|
    puts cyan("Running tests")
    nunit.assemblies testassemblies
    nunit.parameters = [ "--process=Separate" ]
end

desc "Runs the tests with dotcover"
dotcover :test_with_coverage do |dc|
    puts cyan("Running tests with dotcover")
    dc.assemblies testassemblies
    dc.nunitoptions = [ "--process=Separate" ]
    dc.filters "+:module=*;class=*;function=*"
end

def testassemblies
    [
        "bin/Habanero.Test.dll",
        "bin/Habanero.Test.BO.dll",
        "bin/Habanero.Test.DB.dll",
        "bin/Habanero.Test.Structure.dll",
    ]
end

#----------------------------------Copy Tasks----------------------------------

task :copy_to_nuget do
    puts cyan("Copying files to the nuget folder")
    copy_nuget_files_to $nuget_baselocation
end

def copy_nuget_files_to location
    FileSystem.ensure_dir_exists "#{location}/"
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Base.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.BO.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Console.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.DB.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Test.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Test.BO.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Test.Structure.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Test.DB.dll", location

    FileUtils.cp "#{$binaries_baselocation}/Habanero.Base.pdb", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.BO.pdb", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Console.pdb", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.DB.pdb", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Test.pdb", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Test.BO.pdb", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Test.Structure.pdb", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Test.DB.pdb", location
end

#---------------Single Dlls, Nuget Package Push For Internal Use---------------
desc "Pushes Habanero into the given nuget folder"
task :nugetpush => [:publishBaseNugetPackage,
                :publishConsoleNugetPackage,
                :publishDBNugetPackage,
                :publishBONugetPackage,
                :publishTestNugetPackage,
                :publishTestBONugetPackage,
                :publishTestStructureNugetPackage,
                :publishTestDBNugetPackage]

desc "Publish the Habanero.Base nuget package"
pushnugetpackagesonline :publishBaseNugetPackage do |package|
puts cyan("Habanero.Base.#{$nuget_publish_version} ,Version: #{$app_version} ,ApiKey: #{$nuget_apikey},Url: #{$nuget_sourceurl}  ")
  package.InputFileWithPath = "bin/Habanero.Base.dll"
  package.Nugetid = "Habanero.Base.#{$nuget_publish_version}"
  package.Version = $app_version
  package.Description = "Habanero.Base"
  package.ApiKey = "#{$nuget_apikey}"
  package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.BO nuget package"
pushnugetpackagesonline :publishBONugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.BO.dll"
  package.Nugetid = "Habanero.BO.#{$nuget_publish_version}"
  package.Version = $app_version
  package.Description = "Habanero.BO"
  package.ApiKey = "#{$nuget_apikey}"
  package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Console nuget package"
pushnugetpackagesonline :publishConsoleNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Console.dll"
  package.Nugetid = "Habanero.Console.#{$nuget_publish_version}"
  package.Version = $app_version
  package.Description = "Habanero.Console"
  package.ApiKey = "#{$nuget_apikey}"
  package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.DB nuget package"
pushnugetpackagesonline :publishDBNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.DB.dll"
  package.Nugetid = "Habanero.DB.#{$nuget_publish_version}"
  package.Version = $app_version
  package.Description = "Habanero.DB"
  package.ApiKey = "#{$nuget_apikey}"
  package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Test nuget package"
pushnugetpackagesonline :publishTestNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Test.dll"
  package.Nugetid = "Habanero.Test.#{$nuget_publish_version}"
  package.Version = $app_version
  package.Description = "Habanero.Test"
  package.ApiKey = "#{$nuget_apikey}"
  package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Test.BO nuget package"
pushnugetpackagesonline :publishTestBONugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Test.BO.dll"
  package.Nugetid = "Habanero.Test.BO.#{$nuget_publish_version}"
  package.Version = $app_version
  package.Description = "Habanero.Test.BO"
  package.ApiKey = "#{$nuget_apikey}"
  package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Test.Structure nuget package"
pushnugetpackagesonline :publishTestStructureNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Test.Structure.dll"
  package.Nugetid = "Habanero.Test.Structure.#{$nuget_publish_version}"
  package.Version = $app_version
  package.Description = "Habanero.Test.Structure"
  package.ApiKey = "#{$nuget_apikey}"
  package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Test.DB nuget package"
pushnugetpackagesonline :publishTestDBNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Test.DB.dll"
  package.Nugetid = "Habanero.Test.DB.#{$nuget_publish_version}"
  package.Version = $app_version
  package.Description = "Habanero.Test.DB"
  package.ApiKey = "#{$nuget_apikey}"
  package.SourceUrl = "#{$nuget_sourceurl}"
end
