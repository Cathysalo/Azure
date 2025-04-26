
# Peikko - Azure Pipeline YAML Documentation

## **Pipeline Trigger**
The pipeline is triggered when changes are pushed to the **`main` branch**.

```yaml
trigger:
  - main
```

---

## **1. Pool & Variables**
The pipeline runs on **Windows-latest** and uses predefined variables for environment, project name, and deployment settings.

```yaml
pool:
  vmImage: "windows-latest"

variables:
  ENVIRONMENT: dev
  azureProjectName: "pwd-backend"
  projFileName: "PeikkoPrecastWallDesigner.Api/PeikkoPrecastWallDesigner.Api.csproj"
  testProjFileName: "PeikkoPrecastWallDesigner.UnitTests/PeikkoPrecastWallDesigner.UnitTests.csproj"
  workingDirectory: "$(System.DefaultWorkingDirectory)/src"
  buildConfiguration: "Release"
  resourceGroupName: "$(azureProjectName)-$(ENVIRONMENT)-rg"
  webAppName: "$(azureProjectName)-$(ENVIRONMENT)-webapp"
```

---

## **2. Stages & Jobs**

### **Stage 1: Initialization**
- Lists working directory files for debugging.
- Installs **.NET SDK 9**.

```yaml
- stage: Initialize
  displayName: Initialization
  jobs:
  - job: ListFiles
    displayName: List Working Directory
    steps:
    - script: dir "$(System.DefaultWorkingDirectory)"
      displayName: "List files in working directory"
  - job: InstallDotNet
    displayName: Install .NET SDK
    steps:
    - task: UseDotNet@2
      displayName: 'Install .NET SDK 9'
      inputs:
        packageType: 'sdk'
        version: '9.x'
        installationPath: $(Agent.ToolsDirectory)/dotnet
        arguments: "--verbosity detailed"
        includePreviewVersions: true
```

---

### **Stage 2: Restore Packages**
- Restores **NuGet packages** for all `.csproj` files.

```yaml
- stage: Restore
  displayName: Restore Packages
  jobs:
  - job: RestorePackages
    displayName: Restore NuGet Packages
    steps:
    - task: DotNetCoreCLI@2
      displayName: "dotnet restore"
      inputs:
        command: "restore"
        projects: "**/*.csproj"
        feedsToUse: "select"
```

---

### **Stage 3: Build**
- Builds the project in **Release** mode.

```yaml
- stage: Build
  displayName: Build
  dependsOn: Restore
  jobs:
  - job: BuildProjects
    displayName: Build All Projects
    steps:
    - task: DotNetCoreCLI@2
      displayName: "Build All Projects"
      inputs:
        command: "build"
        projects: "**/*.csproj"
        arguments: "--configuration $(buildConfiguration)"
```

---

### **Stage 4: Test**
- Runs **unit tests** and logs results.

```yaml
- stage: Test
  displayName: Test
  dependsOn: Build
  jobs:
  - job: RunTests
    displayName: Unit Tests
    steps:
    - task: DotNetCoreCLI@2
      displayName: "Run Unit Tests"
      inputs:
        command: "test"
        projects: "$(workingDirectory)/$(testProjFileName)"
        arguments: "--configuration $(buildConfiguration) --logger:trx"
```

---

### **Stage 5: Publish & Deploy**
- Publishes the `.NET` project and deploys it to **Azure Web App**.

```yaml
- stage: Deploy
  displayName: Publish and Deploy
  dependsOn: Test
  jobs:
  - job: PublishAndDeploy
    displayName: "Publish and Deploy to Azure"
    steps:
    - task: DotNetCoreCLI@2
      displayName: "Publish .NET Project"
      inputs:
        command: "publish"
        publishWebProjects: false
        projects: "$(workingDirectory)/$(projFileName)"
        arguments: "--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)/$(azureProjectName)_$(ENVIRONMENT)_$(Build.BuildNumber) --verbosity detailed"

    - task: PublishBuildArtifacts@1
      displayName: "Publish Build Artifacts"
      inputs:
        PathtoPublish: "$(Build.ArtifactStagingDirectory)/$(azureProjectName)_$(ENVIRONMENT)_$(Build.BuildNumber)"
        ArtifactName: "$(azureProjectName)_LatestBuild"
        publishLocation: "Container"

    - task: AzureRmWebAppDeployment@4
      displayName: "Deploy to Web App"
      inputs:
        ConnectionType: "AzureRM"
        azureSubscription: "Peikko Services Azure Connection"
        enableCustomDeployment: true
        DeploymentType: "zipDeploy"
        appType: "webApp"
        WebAppName: "$(webAppName)"
        packageForLinux: "$(Build.ArtifactStagingDirectory)/**/*.zip"
        enableCustomApplicationSetting: true
        RemoveAdditionalFilesFlag: true
```

---