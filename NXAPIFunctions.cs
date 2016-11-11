using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;

namespace ClassLibrary1
{
    public enum SaveMode { SaveAndClose, SaveAndNotClose, NotSave};
    public class NXAPIFunctions
    {
        // Принимает на вход массив строк, содержащий данные о выражениях, и экземпляр класса Part,
        // создает выражения в файле детали или, если выражения уже существуют, обновляет их значения,
        public static void ExportExpressions(string[][] expressions_str_arr, Part workPart)
        {
            ExpressionCollection expressions = workPart.Expressions;
            Expression expression;
            Unit unit;
            string exp_type;
            string exp_name;
            string exp_value;
            string exp_unit = "";
            for (int i = 0; i < expressions_str_arr.Length; i++)
            {
                exp_type = expressions_str_arr[i][0];
                exp_name = expressions_str_arr[i][1];
                exp_value = expressions_str_arr[i][2];
                if (expressions_str_arr[i][0] != "Integer" && expressions_str_arr[i][0] != "Point")
                {
                    exp_unit = expressions_str_arr[i][3];
                }
                if (exp_type == "Number")
                {
                    try
                    {
                        expression = expressions.CreateExpression(exp_type, exp_name + " = " + exp_value);
                        unit = (Unit)workPart.UnitCollection.FindObject(exp_unit);
                        expression.Units = unit;
                    }
                    catch (NXException)
                    {
                        expression = (Expression)expressions.FindObject(exp_name);
                        unit = (Unit)workPart.UnitCollection.FindObject(exp_unit);
						expressions.EditWithUnits(expression, unit, exp_value);
                    }
                }
                if (exp_type == "Integer" || exp_type == "Point")
                {
                    try { expression = expressions.CreateExpression(exp_type, exp_name + " = " + exp_value); }
                    catch (NXException)
                    {
                        expression = (Expression)expressions.FindObject(exp_name);
                        expressions.Edit(expression, exp_value); 
                    }
                }
            }
        }
        // сохраняет деталь различными способами
        public static void SavePart(Session theSession, Part workPart, SaveMode save_mode)
        {
            if (save_mode == SaveMode.SaveAndClose)
            {
                workPart.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.True);
            }
            if (save_mode == SaveMode.SaveAndNotClose)
            {
                workPart.Save(BasePart.SaveComponents.True, BasePart.CloseAfterSave.False);
            }
            if (save_mode == SaveMode.NotSave)
            {
                Session.UndoMarkId markId = theSession.SetUndoMark(Session.MarkVisibility.Visible, "Expression");
                int nErrs = theSession.UpdateManager.DoUpdate(markId);
            }           
        }
        // экспортирует тела из файла *.prt в файл файл формата step203
        public static void Step203Export(Session theSession, string part_file_name, string output_file_name)
        {
            Step203Creator step203Creator;
            step203Creator = theSession.DexManager.CreateStep203Creator();
            step203Creator.ObjectTypes.Solids = true;
            step203Creator.SettingsFile = "C:\\Program Files\\Siemens\\NX 10.0\\step203ug\\ugstep203.def";
            step203Creator.InputFile = part_file_name;
            step203Creator.OutputFile = output_file_name;
            step203Creator.FileSaveFlag = false;
            step203Creator.LayerMask = "1-256";
            NXOpen.NXObject nXObject1;
            nXObject1 = step203Creator.Commit();
            step203Creator.Destroy();
        }
    }
}
