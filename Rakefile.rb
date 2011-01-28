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
$:.unshift(File.expand_path(bs)) unless
    $:.include?(bs) || $:.include?(File.expand_path(bs))

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

desc "Builds Habanero, including tests"
task :build => [:clean, :msbuild, :test]

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
	nunit.assemblies 'bin\Habanero.Test.dll','bin\Habanero.Test.Bo.dll','bin\Habanero.Test.Db.dll'
end