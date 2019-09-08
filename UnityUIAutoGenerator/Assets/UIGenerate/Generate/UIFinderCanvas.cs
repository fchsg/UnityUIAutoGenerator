//This script is tool generated. Do not modify it manually

using UIGenerate.Core;
using UnityEngine;

public class UIFinderCanvas : UIFinderBase
{
	public UnityEngine.RectTransform RectTransform_Image { get; private set; }
	public UnityEngine.RectTransform RectTransform_RawImage { get; private set; }
	public UnityEngine.RectTransform RectTransform_Button { get; private set; }
	public UnityEngine.CanvasRenderer CanvasRenderer_Button { get; private set; }
	public UnityEngine.UI.Image Image_Button { get; private set; }
	public UnityEngine.UI.Button Button_Button { get; private set; }

	protected override void Init(Transform trans)
	{
		base.Init(trans);
		RectTransform_Image = trans.Find("Image").GetComponent<UnityEngine.RectTransform>();
		RectTransform_RawImage = trans.Find("RawImage").GetComponent<UnityEngine.RectTransform>();
		RectTransform_Button = trans.Find("Button").GetComponent<UnityEngine.RectTransform>();
		CanvasRenderer_Button = trans.Find("Button").GetComponent<UnityEngine.CanvasRenderer>();
		Image_Button = trans.Find("Button").GetComponent<UnityEngine.UI.Image>();
		Button_Button = trans.Find("Button").GetComponent<UnityEngine.UI.Button>();
	}
}