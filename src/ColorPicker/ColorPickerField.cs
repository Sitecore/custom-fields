using System.Text;
using Sitecore.Diagnostics;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using System;
using System.Text.RegularExpressions;
using Sitecore.Shell.Applications.ContentEditor;

namespace Sitecore.CustomFields.ColorPicker
{
  /// <summary>
  /// Color Picker Field
  /// </summary>
  public class ColorPickerField : Sitecore.Shell.Applications.ContentEditor.Text, IContentField
  {
    #region IContentField
    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public void SetValue(string value)
    {
      Value = value;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The value of the field.</returns>
    public string GetValue()
    {
      return Value;
    }
    #endregion

    #region Properties

    public string Source
    {
      get
      {
        return GetViewStateString("Source");
      }
      set
      {
        Assert.ArgumentNotNull(value, "value");
        SetViewStateString("Source", value);
      }
    }

    public bool UseClassic
    {
      get
      {
        return UIUtil.IsIE();
      }
    }

    #endregion

    #region overrided
    /// <summary>
    /// Handles the message.
    /// </summary>
    /// <param name="message">The message.</param>
    public override void HandleMessage(Message message)
    {
      base.HandleMessage(message);
      if (message["id"] != ID || String.IsNullOrEmpty(message.Name))
      {
        return;
      }

      // in future can be some other message handling
      switch (message.Name)
      {
        case "colorpicker:setcolor":
          Sitecore.Context.ClientPage.Start(this, "SetColor");
          return;
        case "colorpicker:clear":
          Sitecore.Context.ClientPage.Start(this, "ClearColor");
          return;
      }

      if (Value.Length > 0)
      {
        SetModified();
      }

      Value = String.Empty;
    }

    /// <summary>
    /// Renders the control to the specified HTML writer.
    /// </summary>
    /// <param name="output">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
    /// <remarks>When developing custom server controls, you can override this method to generate content for an ASP.NET page.</remarks>
    protected override void Render(System.Web.UI.HtmlTextWriter output)
    {
      base.Render(output);
      output.Write("<object id=\"dlgHelper\" classid=\"clsid:3050f819-98b5-11cf-bb82-00aa00bdce0b\" width=\"0px\" height=\"0px\"></object>");
    }

    #endregion

    #region Message Handlers

    protected void SetColor(ClientPipelineArgs args)
    {
      if (UseClassic)
      {
        SetColorClassic(args);
      }
      else
      {
        SetColorAdvanced(args);
      }
    }

    /// <summary>
    /// Sets the color.
    /// </summary>
    /// <param name="args">The args.</param>
    protected void SetColorClassic(ClientPipelineArgs args)
    {
      if (args.IsPostBack && args.HasResult)
      {
        int oleColor;
        if (int.TryParse(args.Result, out oleColor))
        {
          var colorCode = String.Concat("#", StringUtil.Right("000000" + MainUtil.IntToHex(oleColor), 6));
          if (Value != colorCode)
          {
            SetModified();
            SetValue(colorCode);
          }
        }
      }
      else
      {
        var script = new StringBuilder();
        script.Append("(function(){");
        script.Append("var helper=document.getElementById('dlgHelper');");
        script.Append("if(helper!=null){");
        script.Append("try{");
        script.Append("return helper.ChooseColorDlg(");
        Match iniColor;
        if (!String.IsNullOrEmpty(GetValue()) && (iniColor = Regex.Match(GetValue(), @"(?=^\#(?<small>[0-9a-fA-F]{3})$)|(?=^\#(?<full>[0-9a-fA-F]{6})$)")).Success)
        {
          if (String.IsNullOrEmpty(iniColor.Groups["full"].Value))
          {
            string full = Regex.Replace(iniColor.Groups["small"].Value, "(?<red>[0-9a-fA-F])(?<green>[0-9a-fA-F])(?<blue>[0-9a-fA-F])", "${red}${red}${green}${green}${blue}${blue}");
            script.Append("\'");
            script.Append(full);
            script.Append("\'");
          }
          else
          {
            script.Append("\'");
            script.Append(iniColor.Groups["full"].Value);
            script.Append("\'");
          }
        }
        script.Append(");"); // end ChooseColorDlg
        script.Append("}"); // ent try
        script.Append("catch(err){");
        script.Append("alert(\"Sorry, your browser does not support HtmlDlgSafeHelper, please use IE>=6 version.\");");
        script.Append("}"); // end catch
        script.Append("}"); // end if
        script.Append("})"); // end function
        script.Append("();"); // call function

        SheerResponse.Eval(script.ToString()).Attributes["response"] = "1";
        args.WaitForPostBack();
      }
    }

    protected void SetColorAdvanced(ClientPipelineArgs args)
    {
      if (args.IsPostBack)
      {
        var colorCode = String.Empty;

        if (args.HasResult)
        {
          colorCode = "#" + args.Result;
        }

        if (Value != colorCode)
        {
          SetModified();
        }

        Value = colorCode;
      }
      else
      {
        var url = new UrlString("/sitecore/shell/~/xaml/Sitecore.CustomFields.ColorPicker.ColorPickerDialog.aspx");
        url.Add("value", GetValue());
        SheerResponse.ShowModalDialog(url.ToString(), "365", "524", string.Empty, true);
        args.WaitForPostBack();
      }
    }

    /// <summary>
    /// Clears the color.
    /// </summary>
    /// <param name="args">The args.</param>
    protected void ClearColor(ClientPipelineArgs args)
    {
      SetValue(String.Empty);
      SetModified();
    }
    #endregion
  }
}