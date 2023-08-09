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
        [CommandMethod("JL-CreateLine")]
        public static void CreateLine()
        {
            Document adoc = AcadApp.DocumentManager.MdiActiveDocument;
            Editor ed = adoc.Editor;

            // Prompt user for first point
            // Prompt user for second point
            // Check to make sure points are not identical
            // Create the line
            // Output some helpful message at the end

        }
    }
}
