using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MotinLabelUnityTextMeshWrapper : MotinLabelBaseWrapper {

	TextMesh	textMesh;


	public override string text {
		get {
			if(textMesh!=null)
				return textMesh.text;

			return null;
		}
		set {
			if(textMesh!=null)
				textMesh.text = value;
		}
	}

	protected override void Awake ()
	{
		base.Awake ();
		textMesh = GetComponent<TextMesh>();
	}
}
