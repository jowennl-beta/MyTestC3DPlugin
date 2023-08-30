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

            try
            {
                Line line = LineByTwoPoints(adoc, startPnt, endPnt);
                ed.WriteMessage($"New line created. Length = {line.Length}\n");
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage($"Error: {ex.Message}");
            }

            Stopwatch sw = Stopwatch.StartNew();
            sw.Stop();

            // Output some helpful message at the end
            ed.WriteMessage($"Command complete. Elapsed times = {sw.ElapsedMilliseconds}ms\n");
        }

        public static Line LineByTwoPoints(Document adoc, Point3d startPnt, Point3d endPnt)
        {
            // Check to make sure points are not identical
            if (startPnt == endPnt)
            {
                throw new ArgumentException("Start point and end point cannot be the same.");
            }

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
                }

                tr.Commit();
                return line;
            }
        }

        // Create simple Alignment using 2 points
    }

    
}
