require 'rake'
require 'albacore'

#______________________________________________________________________________
#---------------------------------SETTINGS-------------------------------------

# set up the build script folder so we can pull in shared rake scripts.
# This should be the same for most projects, but if your project is a level
# deeper in the repo you will need to add another ..
bs = File.dirname(__FILE__)
bs = File.join(bs, "../HabaneroCommunity/BuildScripts")
$buildscriptpath = File.expand_path(bs)
$:.unshift($buildscriptpath) unless
    $:.include?(bs) || $:.include?($buildscriptpath)

if (bs.index("branches") == nil)	
	nuget_version = 'Trunk'
	nuget_version_id = '9.9.999'
	
	$nuget_publish_version = nuget_version
	$nuget_publish_version_id = nuget_version_id
else

	$nuget_publish_version = 'v3.0'
	$nuget_publish_version_id = '3.0'
end		
#------------------------build settings--------------------------
require 'rake-settings.rb'

puts cyan("Nuget Details #{$nuget_publish_version }, #{$nuget_publish_version_id} ")	
msbuild_settings = {
  :properties => {:configuration => :debug},
  :targets => [:clean, :rebuild],
  :verbosity => :quiet,
  #:use => :net35  ;uncomment to use .net 3.5 - default is 4.0
}

#------------------------dependency settings---------------------

#------------------------project settings------------------------
$solution = 'source/Habanero.sln'

#______________________________________________________________________________
#---------------------------------TASKS----------------------------------------

desc "Runs the build task"
task :default => [:build]

desc "Builds Habanero, including tests and pushes to local nuget folder"
task :build => [:installNugetPackages, :build_only, :test, :nuget]

desc "Builds Habanero, including tests"
task :build_test => [:clean, :installNugetPackages, :msbuild, :test]

desc "Builds Habanero"
task :build_only => [:clean, :msbuild]

desc "Builds Habanero, including running tests with dotcover then pushes to the local nuget server"
task :build_with_coverage => [:installNugetPackages, :build_only, :test_with_coverage, :nuget]

desc "Pushes Habanero into the local nuget folder"
task :nuget => [:publishBaseNugetPackage, 
				:publishConsoleNugetPackage, 
				:publishDBNugetPackage, 
				:publishBONugetPackage,
				:publishTestNugetPackage,
				:publishTestBONugetPackage,
				:publishTestStructureNugetPackage,
				:publishTestDBNugetPackage]
#------------------------build habanero --------------------

desc "Cleans the bin folder"
task :clean do
	puts cyan("Cleaning bin folder")
	FileUtils.rm_rf 'bin'
  FileSystem.ensure_dir_exists 'bin/Debug'
  FileSystem.ensure_dir_exists 'bin/Release'
end

desc "Builds the solution with msbuild"
msbuild :msbuild do |msb| 
	puts cyan("Building #{$solution} with msbuild")
	msb.update_attributes msbuild_settings
	msb.solution = $solution
end

desc "Runs the tests"
nunit :test do |nunit|
	puts cyan("Running tests")
	nunit.assemblies testassemblies
end

def testassemblies
	['bin\Habanero.Test.dll',
	'bin\Habanero.Test.Bo.dll',
	'bin\Habanero.Test.Db.dll']
end

desc "Runs the tests with dotcover"
dotcover :test_with_coverage do |dc|
	puts cyan("Running tests with dotcover")
	dc.assemblies testassemblies
    dc.filters '+:module=*;class=*;function=*'
end

desc "Install nuget packages"
getnugetpackages :installNugetPackages do |ip|
    ip.package_names = ["nunit.trunk"]
end

desc "Publish the Habanero.Base nuget package"
pushnugetpackages :publishBaseNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Base.dll"
  package.Nugetid = "Habanero.Base.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Base"
end

desc "Publish the Habanero.BO nuget package"
pushnugetpackages :publishBONugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.BO.dll"
  package.Nugetid = "Habanero.BO.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.BO"
end

desc "Publish the Habanero.Console nuget package"
pushnugetpackages :publishConsoleNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Console.dll"
  package.Nugetid = "Habanero.Console.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Console"
end

desc "Publish the Habanero.DB nuget package"
pushnugetpackages :publishDBNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.DB.dll"
  package.Nugetid = "Habanero.DB.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.DB"
end

desc "Publish the Habanero.Test nuget package"
pushnugetpackages :publishTestNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Test.dll"
  package.Nugetid = "Habanero.Test.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Test"
end

desc "Publish the Habanero.Test.BO nuget package"
pushnugetpackages :publishTestBONugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Test.BO.dll"
  package.Nugetid = "Habanero.Test.BO.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Test.BO"
end

desc "Publish the Habanero.Test.Structure nuget package"
pushnugetpackages :publishTestStructureNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Test.Structure.dll"
  package.Nugetid = "Habanero.Test.Structure.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Test.Structure"
end

desc "Publish the Habanero.Test.DB nuget package"
pushnugetpackages :publishTestDBNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Test.DB.dll"
  package.Nugetid = "Habanero.Test.DB.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Test.DB"
end
