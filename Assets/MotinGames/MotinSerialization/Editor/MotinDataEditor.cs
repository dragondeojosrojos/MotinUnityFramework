using UnityEngine;
using UnityEditor;
using System.Collections;
namespace MotinGames

{
	public class MotinDataEditor : MotinEditor  {
		
		
		protected MotinData motinData_ = null;
		protected MotinData motinData
		{
			get {return motinData_;}
			set{
				motinData_ = value;
			}
		}
		
		protected override void targetUpdated()
		{
			//base.targetUpdated();
			if(target==null)
					return;

			motinData = (MotinData)target;
			//editorName = "["+motinData.GetType().ToString() + "] "+ motinData.name;
			
		}
		public MotinDataEditor():base()
		{
			
		}
		public MotinDataEditor(MotinEditor parent):base(parent)
		{
			
		}
		public MotinDataEditor(EditorWindow host,MotinEditor parent): base(host,parent)
		{

		}
		
		// Events
		public System.Action 	OnDataNameChanged;
		public System.Action<MotinEditor> 	OnDataNameChangedWithEditor;
			
		void RaiseNameChanged() { if (OnDataNameChanged != null) OnDataNameChanged(); if (OnDataNameChangedWithEditor != null) OnDataNameChangedWithEditor(this); }
		

		protected override void FieldValueChanged (object value, System.Reflection.FieldInfo field)
		{
			base.FieldValueChanged (value, field);
			if(field.Name =="name")
			{
				RaiseNameChanged();
			}
		}
			/*
		protected override bool DrawField (object value, System.Reflection.FieldInfo field)
		{
			if(field.Name == "name")
			{
				if(DrawDefaultField(value,field))
				{
					RaiseNameChanged();
					//return true;
				}
				return true;
			}
			return base.DrawField (value, field);
		}
		*/
	}
}
