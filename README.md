```
dotnet new sln -n GitExamHooksProject
dotnet new console -n GitExamHooksProject
dotnet new xunit -n GitExamHooksProject.Tests
dotnet sln add GitExamHooksProject/GitExamHooksProject.csproj
dotnet sln add GitExamHooksProject.Tests/GitExamHooksProject.Tests.csproj
dotnet add GitExamHooksProject.Tests/GitExamHooksProject.Tests.csproj reference GitExamHooksProject/GitExamHooksProject.csproj

$ cat GitExamHooksProject/Program.cs
using System;

namespace GitExamHooksProject
{
    public class Program
    {
        public static int Square(int x) => x * x;

        static void Main()
        {
            Console.WriteLine($"Square of 4 is {Square(4)}");
        }
    }
}

$ cat GitExamHooksProject.Tests/FeatureTests.cs
using Xunit;
using GitExamHooksProject;

namespace GitExamHooksProject.Tests
{
    public class FeatureTests
    {
        [Fact]
        public void Square_ReturnsCorrectValue()
        {
            Assert.Equal(16, Program.Square(4));
        }
    }
}


git add .
git commit -m "Initial test commit"

$ cat .editorconfig
root = true

[*.cs]
indent_style = space
indent_size = 4
dotnet_sort_system_directives_first = true
dotnet_style_qualification_for_field = false:suggestion
```
**commit-msg hook:**
```
#!/bin/sh

echo "Running commit-msg hook: checking commit message format..."

commit_msg_file="$1"
commit_msg=$(cat "$commit_msg_file")

# Enforce format: TASK-123: Some message
if ! echo "$commit_msg" | grep -qE '^TASK-[0-9]+: .+'; then
  echo "❌ Commit message format invalid."
  echo "✅ Required format: TASK-<number>: <description>"
  echo "⛔ Example: TASK-101: Fix null reference exception in login"
  exit 1
fi

echo "✅ Commit message format check passed."
exit 0
```
**pre-commit hook:**

```
#!/bin/sh
echo "Running pre-commit hook: checking formatting..."
dotnet format --verify-no-changes
if [ $? -ne 0 ]; then
  echo "❌ Formatting issues detected. Run 'dotnet format'."
  exit 1
fi
echo "✅ Formatting check passed."
exit 0
```

**pre-push hook:**

```
#!/bin/sh
echo "Running pre-push hook: checking build and tests..."
dotnet build --configuration Release
if [ $? -ne 0 ]; then
  echo "❌ Build failed. Push blocked."
  exit 1
fi
dotnet test --no-build --configuration Release
if [ $? -ne 0 ]; then
  echo "❌ Tests failed. Push blocked."
  exit 1
fi
echo "✅ Build and test passed."
exit 0
```

<img width="878" height="599" alt="image" src="https://github.com/user-attachments/assets/54f8f3f9-0175-4e57-ab27-62594d7f75da" />

**Changed logic to contain an error:**

```
$ cat GitExamHooksProject.Tests/FeatureTests.cs
using GitExamHooksProject;
using Xunit;

namespace GitExamHooksProject.Tests
{
    public class FeatureTests
    {
        [Fact]
        public void Square_ReturnsCorrectValue()
        {
            Assert.Equal(17, Program.Square(4));
        }
    }
}
```

<img width="751" height="171" alt="image" src="https://github.com/user-attachments/assets/3b434753-47e8-47ee-9c9f-ddc387add7aa" />

**The push failed due to the broken logic:**

<img width="881" height="459" alt="image" src="https://github.com/user-attachments/assets/9a07b36a-976e-4b75-b3c9-6141182f5d49" />

**The logic was fixed:**

<img width="797" height="170" alt="image" src="https://github.com/user-attachments/assets/0705e19d-1287-47fe-8842-aac07c962220" />

**And the push was successfull:**

<img width="879" height="654" alt="image" src="https://github.com/user-attachments/assets/61a55ce1-4a2a-4e23-b6d7-2caa70030cce" />

**Lower .Net version set:**
```
$ cat GitExamHooksProject/GitExamHooksProject.csproj
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>


$ cat GitExamHooksProject.Tests/GitExamHooksProject.Tests.csproj
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.2" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\GitExamHooksProject\GitExamHooksProject.csproj" />
  </ItemGroup>

</Project>
```
**Workflow YAML file created:**
```
$ cat .github/workflows/dotnet.yml
name: .NET CI

on:
  workflow_dispatch:
  push:
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
```
**Approval to merge to the main branch was set:**
<img width="1352" height="671" alt="image" src="https://github.com/user-attachments/assets/69ee1d11-98ad-4351-9776-fcaefa2531db" />

<img width="1232" height="788" alt="image" src="https://github.com/user-attachments/assets/50166b97-e463-403b-bcab-e0898c22c529" />

**The test were started upon a pull request:**

<img width="933" height="818" alt="image" src="https://github.com/user-attachments/assets/77bd2d2e-f9fb-46a2-8822-5a0d2145b0f7" />
