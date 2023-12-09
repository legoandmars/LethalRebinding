# Lethal Rebinding
Allows rebinding of most controls in Lethal Company

## Known issues

- WASD cannot be changed
- Tooltips/prompts are not updated

## Contributing

1. Fork and clone the repository.
2. Create `LethalRebinding/LethalRebinding.csproj.user`:
    ```xml
    <Project>
      <PropertyGroup>
        <LETHAL_COMPANY_DIR>C:\Program Files (x86)\Steam\steamapps\common\Lethal Company</LETHAL_COMPANY_DIR>
      </PropertyGroup>
      <Target Name="PostBuild" AfterTargets="PostBuildEvent" Condition="false">
        <Exec Command="copy &quot;$(TargetPath)&quot; &quot;$(LETHAL_COMPANY_DIR)\BepInEx\plugins\&quot;" />
      </Target>
    </Project>
    ```
3. Make changes, commit, and push to your fork
4. Open pull request on GitHub