using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
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

            // Prompt user for start point
            PromptPointOptions pointOpts = new PromptPointOptions("\nSelect start point");
            PromptPointResult pointRes = ed.GetPoint(pointOpts);
            
            // Check for valid input
            if (pointRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("Cannot proceed\n");
                return;
            }

            Point3d startPnt = pointRes.Value;

            // Prompt user for second point
            pointOpts.Message = "\nSelect end point";
            pointRes = ed.GetPoint(pointOpts);

            // Check for valid input
            if (pointRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("Cannot proceed\n");
                return;
            }

            Point3d endPnt = pointRes.Value;

            // Check to make sure points are not identical
            if (startPnt == endPnt)
            {
                ed.WriteMessage("Error: Cannot create line with identical start and end points.\n");
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();

            // Create the line
            using (Transaction tr = adoc.TransactionManager.StartTransaction())
            {
                BlockTable bt = tr.GetObject(adoc.Database.BlockTableId, OpenMode.ForWrite) as BlockTable;
                BlockTableRecord ms = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                // Another way to do it without getting model space Object ID from Block Table
                //BlockTableRecord ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(adoc.Database), OpenMode.ForWrite) as BlockTableRecord;

                Line line = new Line(startPnt, endPnt);
                if (line != null)
                {
                    ms.AppendEntity(line);
                    tr.AddNewlyCreatedDBObject(line, true);
                    ed.WriteMessage($"New line created. Length = {line.Length}\n");
                }
                
                tr.Commit();             
            }

            sw.Stop();

            // Output some helpful message at the end
            ed.WriteMessage($"Command complete. Elapsed times = {sw.ElapsedMilliseconds}ms\n");
        }

        // Next step: refactor the code to extract the line creation into a separate method and call within CommandMethod
        // Create simple Alignment using 2 points
    }
}
