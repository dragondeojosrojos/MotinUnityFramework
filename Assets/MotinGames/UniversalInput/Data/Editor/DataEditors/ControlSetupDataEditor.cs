using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using MotinGames;

public class ControlSetupDataEditor : MotinDataEditor  {
	
	protected MotinArrayListEditor		bindingsListEditor = null;
	ControlSetupData		setupData = null;
	
	protected override void targetUpdated ()
	{
		base.targetUpdated ();
		setupData = (ControlSetupData)motinData_;
		
	}
	
	protected override void Initialize ()
	{
		base.Initialize ();
		
		bindingsListEditor = new MotinArrayListEditor(hostEditorWindow,typeof(InputActionBindingData));
		bindingsListEditor.OnFieldValueChanged+=OnBindingDataChanged;
		bindingsListEditor.editorName = "Control Bindings";
		//projectCostArrayEditor.orderDatas = false;
		bindingsListEditor.editorColor = Color.cyan;
	}
	
	protected override bool DrawField (object value, System.Reflection.FieldInfo field)
	{
		if(field.Name =="inputBindings")
		{
			bindingsListEditor.target = field.GetValue(value);
			bindingsListEditor.Draw(new Rect(0,0,0,3000),true);
			return true;
		}
		return base.DrawField (value, field);
	}
	
	
	
	void OnBindingDataChanged(object instance,System.Reflection.FieldInfo field)
	{
		setupData.inputBindings = bindingsListEditor.objectList.Cast<InputActionBindingData>().ToList();
		RaiseOnFieldValueChanged(target,null);
	}
}