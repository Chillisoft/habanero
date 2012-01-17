require 'rake'
require 'albacore'

#______________________________________________________________________________
#---------------------------------SETTINGS-------------------------------------

# set up the build script folder so we can pull in shared rake scripts.
# This should be the same for most projects, but if your project is a level
# deeper in the repo you will need to add another ..
bs = File.dirname(__FILE__)
bs = File.join(bs, "..") if bs.index("branches") != nil
bs = File.join(bs, "../../HabaneroCommunity/BuildScripts")
$buildscriptpath = File.expand_path(bs)
$:.unshift($buildscriptpath) unless
    $:.include?(bs) || $:.include?($buildscriptpath)

#------------------------build settings--------------------------
require 'rake-settings.rb'

msbuild_settings = {
  :properties => {:configuration => :release},
  :targets => [:clean, :rebuild],
  :verbosity => :quiet,
  #:use => :net35  ;uncomment to use .net 3.5 - default is 4.0
}

#------------------------dependency settings---------------------

#------------------------project settings------------------------
$basepath = 'http://delicious:8080/svn/habanero/Habanero/trunk'
$solution = 'source/Habanero.sln'

#______________________________________________________________________________
#---------------------------------TASKS----------------------------------------

desc "Runs the build task"
task :default => [:build]

desc "Builds Habanero, including tests and pushes to local nuget folder"
task :build => [:build_only, :test, :nuget]

desc "Builds Habanero, including tests"
task :build_test => [:clean, :msbuild, :test]

desc "Builds Habanero"
task :build_only => [:clean, :msbuild]

desc "Builds Habanero, including running tests with dotcover"
task :build_with_coverage => [:build_only, :test_with_coverage]

desc "Pushes Habanero into the local nuget folder"
task :nuget => [:publishBaseNugetPackage, :publishConsoleNugetPackage, :publishDBNugetPackage, :publishBONugetPackage ]
#------------------------build habanero --------------------

desc "Cleans the bin folder"
task :clean do
	puts cyan("Cleaning bin folder")
	FileUtils.rm_rf 'bin'
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
	['bin\Habanero.Test.dll','bin\Habanero.Test.Bo.dll','bin\Habanero.Test.Db.dll']
end

desc "Runs the tests with dotcover"
dotcover :test_with_coverage do |dc|
	puts cyan("Running tests with dotcover")
	dc.assemblies testassemblies
    dc.filters '+:module=*;class=*;function=*'
end


desc "Publish the Habanero.Base nuget package"
pushnugetpackages :publishBaseNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Base.dll"
  package.Nugetid = "Habanero.Base.Trunk"
  package.Version = "Trunk"
  package.Description = "Habanero.Base"
end

desc "Publish the Habanero.BO nuget package"
pushnugetpackages :publishBONugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.BO.dll"
  package.Nugetid = "Habanero.BO.Trunk"
  package.Version = "Trunk"
  package.Description = "Habanero.BO"
end

desc "Publish the Habanero.Console nuget package"
pushnugetpackages :publishConsoleNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Console.dll"
  package.Nugetid = "Habanero.Console.Trunk"
  package.Version = "Trunk"
  package.Description = "Habanero.Console"
end

desc "Publish the Habanero.DB nuget package"
pushnugetpackages :publishDBNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.DB.dll"
  package.Nugetid = "Habanero.DB.Trunk"
  package.Version = "Trunk"
  package.Description = "Habanero.DB"
end