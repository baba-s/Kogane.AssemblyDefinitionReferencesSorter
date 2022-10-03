using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Kogane
{
    public static class AssemblyDefinitionReferencesSorter
    {
        private sealed class Data
        {
            public  string Reference { get; }
            private string GUID      { get; }
            private string AssetPath { get; }
            public  string FileName  { get; }

            public bool IsValid =>
                !string.IsNullOrEmpty( GUID ) &&
                !string.IsNullOrEmpty( AssetPath ) &&
                !string.IsNullOrEmpty( FileName );

            public Data( string reference )
            {
                Reference = reference;
                GUID      = reference.Replace( "GUID:", "" );
                AssetPath = AssetDatabase.GUIDToAssetPath( GUID ) ?? string.Empty;
                FileName  = Path.GetFileName( AssetPath );
            }
        }

        public static void Sort( string assetPath )
        {
            var json          = File.ReadAllText( assetPath );
            var jsonData      = JsonUtility.FromJson<JsonAssemblyDefinition>( json );
            var oldReferences = jsonData.references;

            var newReferences = oldReferences
                    .Select( x => new Data( x ) )
                    .OrderBy( x => !x.IsValid )
                    .ThenBy( x => x.FileName, new NaturalComparer() )
                    .Select( x => x.Reference )
                    .ToArray()
                ;

            if ( newReferences.SequenceEqual( oldReferences ) ) return;

            jsonData.references = newReferences;

            File.WriteAllText
            (
                assetPath,
                JsonUtility.ToJson( jsonData, true ),
                Encoding.UTF8
            );

            var asset = AssetDatabase.LoadAssetAtPath<Object>( assetPath );

            EditorUtility.SetDirty( asset );
        }
    }
}