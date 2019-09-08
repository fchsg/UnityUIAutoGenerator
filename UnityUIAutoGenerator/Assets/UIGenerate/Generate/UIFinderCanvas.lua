--此脚本为工具生成，请勿手动修改

local UIFinderCanvas  = { }

UIFinderCanvas.RectTransform_Image = nil
UIFinderCanvas.RectTransform_RawImage = nil
UIFinderCanvas.RectTransform_Button = nil
UIFinderCanvas.CanvasRenderer_Button = nil
UIFinderCanvas.Image_Button = nil
UIFinderCanvas.Button_Button = nil

function UIFinderCanvas:Init(trans)
	self.RectTransform_Image = trans:Find("Image"):GetComponent("RectTransform")
	self.RectTransform_RawImage = trans:Find("RawImage"):GetComponent("RectTransform")
	self.RectTransform_Button = trans:Find("Button"):GetComponent("RectTransform")
	self.CanvasRenderer_Button = trans:Find("Button"):GetComponent("CanvasRenderer")
	self.Image_Button = trans:Find("Button"):GetComponent("Image")
	self.Button_Button = trans:Find("Button"):GetComponent("Button")
end

return UIFinderCanvas