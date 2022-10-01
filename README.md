# Kogane Assembly Definition References Sorter

AssemblyDefinitionAsset の References をファイル名順に並べ替えるクラス

## 使用例

```csharp
using Kogane;
using UnityEditor;

public static class Example
{
    [MenuItem( "Tools/Hoge" )]
    public static void Hoge()
    {
        var path = AssetDatabase.GetAssetPath( Selection.activeObject );
        AssemblyDefinitionReferencesSorter.Sort( path );
        AssetDatabase.Refresh();
    }
}
```

## 依存しているパッケージ

* https://github.com/baba-s/Kogane.JsonAssemblyDefinition
* https://github.com/baba-s/Kogane.NaturalComparer