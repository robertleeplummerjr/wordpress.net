using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PHP;
using PHP.Core;
using PHP.Library;
using PHP.Library.Data;

namespace wordpress.net
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var c = ScriptContext.CurrentContext;
            var output = new Literal();
            
            //first load in assemblies
            ApplicationContext.Default.AssemblyLoader.Load(typeof(PHP.Library.PhpConstants).Assembly, null);
            ApplicationContext.Default.AssemblyLoader.Load(typeof(PHP.Library.Data.MySql).Assembly, null);

            //setup WordPress
            Setup(c);

            //initiate WordPress
            c.Include("..\\components\\WordPress\\index.php", true);
            
            output.Text = c.BufferedOutput.GetContentAsString();

            Page.Controls.Add(output);
        }

        protected void Setup(ScriptContext c)
        {
            // Set a variable to the My Documents path.
            var section = (ClientSettingsSection)ConfigurationManager
                .GetSection("applicationSettings/wordpress.net.Properties.Settings");
            var settings = section.Settings;

            foreach (SettingElement setting in settings)
            {
                var value = setting.Value.ValueXml.InnerText;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    c.DefineConstant(setting.Name, setting.Value.ValueXml.InnerText);
                }
                else
                {
                    switch (setting.Name)
                    {
                        case "ABSPATH":
                            var path = System.IO.Directory.GetParent(Server.MapPath("~")).Parent.FullName +
                                       "\\components\\WordPress\\";
                            c.DefineConstant("ABSPATH", path.Replace("\\", "/"));
                            break;
                    }
                }
            }

            /** Sets up WordPress vars and included files. */
            c.Include("..\\components\\WordPress\\wp-settings.php", true);
        }
    }
}