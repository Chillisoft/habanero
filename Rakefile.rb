require 'rake'
require 'albacore'
    
task :default => [:build_habanero] #this is the starting point for the script

task :build_habanero => [:clean_habanero,:checkout_habanero,:msbuild_habanero,:run_ncover,:run_ncoverexp] # a list of tasks spawned off the default task

$Ncover_path = "C:/Program Files (x86)/NCover/NCover.Console.exe"
$Nunit_path = %q("C:/Program Files (x86)/NUnit 2.5.6/bin/net-2.0/nunit-console-x86.exe")
$NcoverExp_path = "c:/program files (x86)/ncoverexplorer/ncoverexplorer.console.exe"

#build_habanero tasks
task :clean_habanero do #deletes bin folder
	FileUtils.rm_rf 'bin'
end

exec :checkout_habanero do |cmd| #command to check out habanero source using SVN
	cmd.path_to_command = "../../Utilities/BuildServer/Subversion/bin/svn.exe" # for some reason this doesn't pick up environment variables so I can't just use 'svn'
	cmd.parameters %q(checkout "http://delicious:8080/svn/habanero/Habanero/trunk" ./ --username chilli --password chilli) 
end

msbuild :msbuild_habanero do |msb| #builds habanero with msbuild
    msb.targets :Build
  msb.solution = "source/Habanero.sln"
  msb.verbosity = "quiet"
  end

ncoverconsole :run_ncover do |ncc| #This runs the ncover and (hopefully nunit) stuff
 ncc.path_to_command = $Ncover_path
 ncc.output :xml => "Coverage.xml"
 ncc.working_directory = "."
 ncc.cover_assemblies("Habanero.BO")
 #ncc.ignore_assemblies("Habanero.Base.CoverageExcludeAttribute") #this only works with the commercial version of ncover
 
#trigger Nunit
  nunit = NUnitTestRunner.new($Nunit_path)
# nunit-console-x86.exe is run to prevent the "Profiler connection not established" error from old Ncover versions
 nunit.assemblies 'bin\Habanero.Test.dll','bin\Habanero.Test.Bo.dll','bin\Habanero.Test.Db.dll'

#nunit.options '/labels'
#enable this if you want to see pretty test names scrolling in the hudson page while it builds
 
#set Nunit as the test runner
 ncc.testrunner = nunit
end

exec :run_ncoverexp do |cmd| # uses a batch file to run ncover because I ran into some very wierd stuff trying to get the parameters to run on an exec task
	cmd.path_to_command = $NcoverExp_path
	cmd.parameters "coverage.xml /html /r:4 /p:Habanero.Core"
end
