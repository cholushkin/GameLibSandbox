using UnityEngine;

namespace MoonSharp.Interpreter.Loaders
{
	public class UnityAssetsScriptLoader : ScriptLoaderBase
	{
		public override object LoadFile(string file, Table globalContext)
        {
            var scriptTextAsset = Resources.Load<TextAsset>(file);
            return scriptTextAsset.text;
        }
										
		public override bool ScriptFileExists(string file)
        {
            return Resources.Load<TextAsset>(file) != null;
		}
	}
}

