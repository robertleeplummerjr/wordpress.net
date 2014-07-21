using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PHP;
using PHP.Core;
using PHP.Library;

namespace wordpress.net
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var c = ScriptContext.CurrentContext;
            var output = new Literal();
            ApplicationContext.Default.AssemblyLoader.Load(typeof(PHP.Library.PhpConstants).Assembly, null);
            ApplicationContext.Default.AssemblyLoader.Load(typeof(PHP.Library.Data.MySql).Assembly, null);
            
            c.Include("..\\components\\WordPress\\index.php", true);
            
            output.Text = c.BufferedOutput.GetContentAsString();

            Page.Controls.Add(output);
        }
    }
}