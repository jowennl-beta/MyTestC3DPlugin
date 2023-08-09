using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace MyTestC3DPlugin
{
    public class MyCommands
    {
        [CommandMethod("JL-TestCommand")]
        public static void TestCommand()
        {
            Document adoc = AcadApp.DocumentManager.MdiActiveDocument;
            Editor ed = adoc.Editor;

            ed.WriteMessage("Hello world!");
        }
    }
}
