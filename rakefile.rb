require 'albacore'
require 'version_bumper'
bumper_file "version.txt"

# Read any atypical version bump type from environment/command-line.
# e.g., rake bumpType=minor
# bumpType=major -> 2.3.251.0 becomes 3.0.0.0
# bumpType=minor -> 2.3.251.0 becomes 2.4.0.0
# no bumpType specified -> 2.3.251.0 becomes 2.3.252.0
bumpType = ENV["bumpType"] || 'revision'

# Setup variables.
assemblyName = "SecuritySwitch"
projectDir = "Source/SecuritySwitch/"
targetFile = "#{projectDir}bin/Release/#{assemblyName}.dll"
targetStrongFile = "#{projectDir}bin/Strong/#{assemblyName}.dll"
keyFile = "#{assemblyName}.snk"

productName = "Security Switch"
productDescription = ".NET libraries for automatically switching between HTTP and HTTPS protocols."
productAuthors = "Matt Sollars"
copyrightBeginYear = 2004

nugetId = "SecuritySwitch"
nugetDir = "NuGet/"
nuspecFile = "#{nugetDir}#{assemblyName}.nuspec"

configSchemaFile = "#{assemblyName}-v#{bumper_version.major}.xsd"

downloadsDir = "Downloads/"
workingDir = "#{downloadsDir}Working"
archiveName = "#{assemblyName} v#{bumper_version.to_s} - Binary"
archiveStrongName = "#{assemblyName} v#{bumper_version.to_s} - Strongly Named Binary"


# Start the build.
task :default => [:build, :pushNuGet, :createArchives]
#task :default => [:build, :pushNuGet, :uploadArchives]


desc "Sets common assembly information."
assemblyinfo :setAssemblyInfo do |asm|
	case bumpType
		when 'major'
			Rake::Task['bump:major'].invoke
		when 'minor'
			Rake::Task['bump:minor'].invoke
		when 'revision'
			Rake::Task['bump:revision'].invoke
		when 'none'
			# no-op
		else
			puts "Unknown bumpType. Exiting."
			exit 0
	end
	
	asm.version = bumper_version.to_s
	asm.file_version = bumper_version.to_s

	now = Time.new
	asm.product_name = productName
	asm.copyright = "Copyright (c) #{copyrightBeginYear}-#{now.year} #{productAuthors}"
	asm.description = productDescription
	
	asm.com_visible = false
	asm.output_file = "CommonAssemblyInfo.cs"
end

desc "Builds the solution for a new release."
msbuild :build => :setAssemblyInfo do |msb|
	msb.properties = { :configuration => :Release }
	msb.targets = [:Clean, :Build]
	msb.solution = "SecuritySwitch.sln"
end

desc "Builds the solution for a new strongly named release."
msbuild :buildStrong => :setAssemblyInfo do |msb|
	msb.properties = { 
		:configuration => :Strong, 
		:signAssembly => "true", 
		:assemblyOriginatorKeyFile => "../../#{keyFile}"
	}
	msb.targets = [:Build]
	msb.solution = "#{projectDir}SecuritySwitch.csproj"
end

desc "Runs tests for solution."
xunit :runTests do |xunit|
	xunit.command = "packages/xunit.runners.1.9.1/tools/xunit.console.exe"
	xunit.assembly = "Source/Tests/bin/Release/#{assemblyName}.Tests.dll"
end

desc "Creates the NuGet specification."
nuspec :createNuSpec do |spec|
	spec.id = nugetId
	spec.version = bumper_version.to_s
	spec.title = productName
	spec.authors = productAuthors
	spec.description = productDescription
	spec.language = "en-US"
	spec.projectUrl = "http://code.google.com/p/securityswitch/"
	spec.tags = "security switch secure SSL HTTPS ASP.NET MVC"
	
	spec.output_file = nuspecFile
end

desc "Create the NuGet package."
nugetpack :packNuGet => [:runTests, :createNuSpec] do |nuget|
	# Copy NuGet content files (config schema).
	FileUtils.cp configSchemaFile, "#{nugetDir}content/"
	
	# Copy NuGet lib files (output files/assemblies).
	FileUtils.cp targetFile, "#{nugetDir}lib/"
	
	# Pack NuGet spec file; output to the NuGet directory.
	nuget.nuspec = nuspecFile
	nuget.base_folder = nugetDir
	nuget.output = nugetDir
end

desc "Pushes the NuGet package."
nugetpush :pushNuGet => :packNuGet do |nuget|
	nuget.package = "#{nugetDir}#{assemblyName}.#{bumper_version.to_s}.nupkg"
end


desc "Sets up the working directory for archive creation."
task :setupWorkingDir do
	FileUtils.mkpath workingDir

	FileUtils.rm Dir.glob("#{workingDir}/*")
	
	FileUtils.cp "License.txt", workingDir
	FileUtils.cp "ReadMe.txt", workingDir
	FileUtils.cp configSchemaFile, workingDir
end

desc "Creates the standard archive."
zip :createStandardArchive do |zip|
	FileUtils.cp targetFile, workingDir
	
	zip.directories_to_zip workingDir
	zip.output_file = "#{archiveName}.zip"
	zip.output_path = downloadsDir
	
	puts "Archive created: #{downloadsDir}#{archiveName}.zip"
end

desc "Creates the strongly named archive."
zip :createStrongArchive do |zip|
	FileUtils.cp targetStrongFile, workingDir
	
	zip.directories_to_zip workingDir
	zip.output_file = "#{archiveStrongName}.zip"
	zip.output_path = downloadsDir
	
	puts "Archive created: #{downloadsDir}#{archiveStrongName}.zip"
end

desc "Creates the download archives."
task :createArchives => [:build, :runTests, :buildStrong, :setupWorkingDir, :createStandardArchive, :createStrongArchive]


desc "Uploads the archive(s) to Google Code."
#task :uploadArchives => :createArchives do
task :uploadArchives do
	uploadUrl = "https://uploads.code.google.com/upload/securityswitch"
	userAgent = "Uploader via cURL"
	fields = { 
		"summary" => archiveName, 
		"description" => "Latest stable build of SecuritySwitch v#{bumper_version.major}.#{bumper_version.minor}.", 
		#"file" => "@#{downloadsDir}#{archiveName}.zip"
		"file" => "@#{downloadsDir}test.txt"
	}
	labels = ["Type-Archive", "Featured"]
	
	fieldCommands = ""
	#labels.each { |label| fieldCommands += "--form 'label=#{label}' " }
	#fields.each { |key, value| fieldCommands += "--form '#{key}=#{value}' " }
	verbose(true) do
		sh "curl --basic " + fieldCommands + "#{uploadUrl}"
	end
end