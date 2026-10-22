#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

// Runs automatically after every iOS build and patches the generated Xcode project
// so it compiles with recent Xcode versions
public static class DisableXcodeScriptSandboxing
{
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS) return;

        string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        var project = new PBXProject();
        project.ReadFromFile(projectPath);

        string[] guids =
        {
            project.ProjectGuid(),
            project.GetUnityMainTargetGuid(),
            project.GetUnityFrameworkTargetGuid(),
            project.TargetGuidByName("GameAssembly")
        };

        foreach (string guid in guids)
        {
            if (string.IsNullOrEmpty(guid)) continue;

            // Fix 1: "Sandbox: deny file-write/file-read" during the IL2CPP step
            project.SetBuildProperty(guid, "ENABLE_USER_SCRIPT_SANDBOXING", "NO");

            // Fix 2: "VerifyModule ... could not build module" on UnityFramework headers
            project.SetBuildProperty(guid, "ENABLE_MODULE_VERIFIER", "NO");
            project.SetBuildProperty(guid, "CLANG_WARN_QUOTED_INCLUDE_IN_FRAMEWORK_HEADER", "NO");
        }

        project.WriteToFile(projectPath);
    }
}
#endif