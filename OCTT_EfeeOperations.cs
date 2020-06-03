#region Open Course Timetabler - An application for school and university course timetabling
//
// Author:
//   Ivan Æurak (mailto:Ivan.Curak@fesb.hr)
//
// Copyright (c) 2007 Ivan Æurak, Split, Croatia
//
// http://www.openctt.org
//
//This file is part of Open Course Timetabler.
//
//Open Course Timetabler is free software;
//you can redistribute it and/or modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2 of the License,
//or (at your option) any later version.
//
//Open Course Timetabler is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
//or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//You should have received a copy of the GNU General Public License along with
//Open Course Timetabler; if not, write to the Free Software Foundation, Inc., 51 Franklin St,
//Fifth Floor, Boston, MA  02110-1301  USA

#endregion


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Data;

using MySql.Data.MySqlClient;

using OCTTPluginInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace OCTT_Efee_Plugin
{
    /// <summary>
    /// Summary description for OCTT_EfeeOperations.
    /// </summary>
    public class OCTT_EfeeOperations
    {
        public OCTT_EfeeOperations()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void DoExportJson(BackgroundWorker worker, DoWorkEventArgs e)
        {
            DialogResult result;
            string message = "Are you sure you want to export data to JSON file?";

            string caption = "Exporting to JSON format";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            result = MessageBox.Show(message, caption, buttons,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                JObject timetable = exportJson(new Action[]
                {
                    () => worker.ReportProgress(0, ""),
                    () => worker.ReportProgress(3, "Status: Table with data about timetable is filled up."),
                    () => worker.ReportProgress(6, "Status: Table with data about days is filled up."),
                    () => worker.ReportProgress(12, "Status: Table with data about time periods is filled up."),
                    () => worker.ReportProgress(25, "Status: Table with data about teachers is filled up."),
                    () => worker.ReportProgress(31, "Status: Table with data about rooms is filled up."),
                    () => worker.ReportProgress(37, "Status: Table with data about educational program groups is filled up with data."),
                    () => worker.ReportProgress(50, "Status: Table with data about educational programs is filled up with data."),
                    () => worker.ReportProgress(75, "Status: Table with data about courses is filled up with data."),
                    () => worker.ReportProgress(93, "Status: Table with data about allocated lessons is filled up with data.")
                });
                string outputFullName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\timetable.json"; ;
                File.WriteAllText(outputFullName, timetable.ToString());
                string args = string.Format("/e, /select, \"{0}\"", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\timetable.json");
                ProcessStartInfo info = new ProcessStartInfo { FileName = "explorer", Arguments = args };
                Process.Start(info);
                worker.ReportProgress(100, "Status: Export to JSON file is finished successfully!");
            }
        }

        private static JObject exportJson(Action[] actions)
        {
            IPluginHost host = OCTT_EfeeTTForm.OCTT_EFEE_EXPLG.Host;
            DataSet ds = host.OpenCTTDataSet;

            actions[0]();
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var timetable = new JObject();

                //export document properties
                DataTable dtDocumentProperties = ds.Tables["DocumentProperties"];
                DataRow drDP = dtDocumentProperties.Rows[0];

                var document_props = new JObject();

                document_props.Add("type", int.Parse(drDP["DocType"].ToString()));
                document_props.Add("institution_name", drDP["DocEduInstitutionName"].ToString());
                document_props.Add("school_year", drDP["DocSchoolYear"].ToString());
                document_props.Add("last_change", drDP["DocDateTimeOfLastChange"].ToString());

                timetable.Add("document_props", document_props);

                actions[1]();

                //export included days
                DataTable dtIncludedDays = ds.Tables["IncludedDays"];
                var octt_day = new JArray();
                foreach (DataRow dr in dtIncludedDays.Rows)
                {
                    var jDr = new JObject();

                    jDr.Add("name", dr["DayName"].ToString());
                    jDr.Add("day_index", int.Parse(dr["DayIndexInWeek"].ToString()));

                    octt_day.Add(jDr);
                }
                timetable.Add("octt_day", octt_day);

                actions[2]();


                //export included terms
                DataTable dtIncludedTerms = ds.Tables["IncludedTerms"];
                var octt_term = new JArray();
                foreach (DataRow dr in dtIncludedTerms.Rows)
                {
                    octt_term.Add(new JObject
                        {
                            { "start_h", int.Parse(dr["StartH"].ToString()) },
                            { "start_min", int.Parse(dr["StartM"].ToString()) },
                            { "end_h", int.Parse(dr["EndH"].ToString()) },
                            { "end_min", int.Parse(dr["EndM"].ToString()) },
                            { "term_index", int.Parse(dr["TermIndex"].ToString()) }
                        });
                }
                timetable.Add("octt_term", octt_term);

                actions[3]();

                //export teachers
                DataTable dtTeachers = ds.Tables["Teachers"];
                var octt_teacher = new JArray();
                foreach (DataRow dr in dtTeachers.Rows)
                {
                    var obj = new JObject()
                        {
                            {"name", dr["Name"]?.ToString()},
                            {"lastname", dr["Lastname"]?.ToString()},
                            {"title", dr["Title"]?.ToString()},
                            {"edurank", dr["EduRank"]?.ToString()}
                        };
                    if (!string.IsNullOrEmpty(dr["ExtId"]?.ToString()))
                        obj.Add("ext_id", int.Parse(dr["ExtId"]?.ToString()));
                    octt_teacher.Add(obj);
                }
                timetable.Add("octt_teacher", octt_teacher);

                actions[5]();


                //export rooms
                DataTable dtRooms = ds.Tables["Rooms"];
                var octt_room = new JArray();
                foreach (DataRow dr in dtRooms.Rows)
                {
                    var obj = new JObject()
                        {
                            {"room_id",int.Parse(dr["ID"]?.ToString())},
                            {"name", dr["Name"]?.ToString()},
                            {"capacity",int.Parse(dr["Capacity"]?.ToString())}
                        };
                    if (!string.IsNullOrEmpty(dr["ExtId"]?.ToString()))
                        obj.Add("ext_id", int.Parse(dr["ExtId"]?.ToString()));
                    octt_room.Add(obj);
                }
                timetable.Add("octt_room", octt_room);

                actions[6]();


                //export edu program groups
                DataTable dtEduProgramGroups = ds.Tables["EduProgramGroups"];
                var epg = new JArray();
                foreach (DataRow dr in dtEduProgramGroups.Rows)
                {
                    var obj = new JObject
                        {
                            {"name", dr["Name"]?.ToString()},
                        };
                    if (!string.IsNullOrEmpty(dr["ExtId"]?.ToString()))
                        obj.Add("ext_id", int.Parse(dr["ExtId"]?.ToString()));
                    epg.Add(obj);
                }
                timetable.Add("epg", epg);

                actions[7]();


                //export edu programs
                DataTable dtEduPrograms = ds.Tables["EduPrograms"];
                var ep = new JArray();
                foreach (DataRow dr in dtEduPrograms.Rows)
                {
                    var obj = new JObject
                        {
                            {"name", dr["Name"]?.ToString()},
                            { "code", int.Parse(dr["Code"]?.ToString())},
                            { "semestar", int.Parse(dr["Semester"]?.ToString())},
                            { "epg_id", int.Parse(dr["EpgID"]?.ToString())}
                        };
                    if (!string.IsNullOrEmpty(dr["ExtId"]?.ToString()))
                        obj.Add("ext_id", int.Parse(dr["ExtId"]?.ToString()));
                    ep.Add(obj);
                }
                timetable.Add("ep", ep);

                actions[8]();


                //export courses
                DataTable dtCourses = ds.Tables["Courses"];
                var course = new JArray();
                foreach (DataRow dr in dtCourses.Rows)
                {
                    var obj = new JObject
                        {
                            {"name", dr["Name"]?.ToString()},
                            { "short_name", dr["ShortName"]?.ToString()},
                            { "teacher_id", int.Parse(dr["TeacherID"]?.ToString())},
                            { "group_name", dr["GroupName"]?.ToString()},
                            { "numoflessperweek", int.Parse(dr["NumOfLessPerWeek"]?.ToString())},
                            { "ep_id", int.Parse(dr["EpID"]?.ToString())},
                            { "course_type", dr["CourseType"]?.ToString()}
                        };
                    if (!string.IsNullOrEmpty(dr["ExtId"]?.ToString()))
                        obj.Add("ext_id", int.Parse(dr["ExtId"]?.ToString()));
                    course.Add(obj);
                }
                timetable.Add("course", course);

                actions[9]();


                //export lessons in timetable
                DataTable dtLessonsInTT = ds.Tables["LessonsInTT"];
                var allocated_lesson = new JArray();
                foreach (DataRow dr in dtLessonsInTT.Rows)
                {
                    allocated_lesson.Add(new JObject
                        {
                            {"course_id", int.Parse(dr["CourseID"]?.ToString())},
                            {"day_id", int.Parse(dr["DayID"]?.ToString())},
                            { "term_id", int.Parse(dr["TermId"]?.ToString())},
                            { "room_id", int.Parse(dr["RoomId"]?.ToString())}
                        });
                }
                timetable.Add("allocated_lesson", allocated_lesson);

                return timetable;
            }
            catch (Exception ex)
            {
                string mess = "Post on server was not successfull!\nAn error occurred.\n\n";
                mess += ex.Message + "\n" + ex.ToString();
                mess += "\n" + ex.StackTrace;

                MessageBox.Show(mess, "Error");
            }
            return null;
        }

        public static void DoPostOnServer(BackgroundWorker worker, DoWorkEventArgs e, TextBox[] boxes,
            Button[] buttonsToDisable)
        {
            IPluginHost host = OCTT_EfeeTTForm.OCTT_EFEE_EXPLG.Host;
            DataSet ds = host.OpenCTTDataSet;

            var buts = buttonsToDisable.ToList().Select(button => new { button, button.Enabled }).ToList();
            buts.ForEach(elem => elem.button.Enabled = false);
            try
            {
                worker.ReportProgress(0, "");
                string message = "Connection to database was successfull.\n\n";
                message += "If you proceed with this operation, all the data in tables on server\n";
                message += "epg, ep, course, allocated_lesson, day, term, teacher, room\n";
                message += "will be deleted and after that these tables will be filled up with new data.\n\n";
                message += "Are you sure you want to proceed?";

                string caption = "Confirm to delete existing data";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    JObject timetable = exportJson(new Action[]
                    {
                        () => worker.ReportProgress(0, ""),
                        () => worker.ReportProgress(3, "Status: Table with data about timetable is filled up."),
                        () => worker.ReportProgress(6, "Status: Table with data about days is filled up."),
                        () => worker.ReportProgress(12, "Status: Table with data about time periods is filled up."),
                        () => worker.ReportProgress(25, "Status: Table with data about teachers is filled up."),
                        () => worker.ReportProgress(31, "Status: Table with data about rooms is filled up."),
                        () => worker.ReportProgress(35, "Status: Table with data about educational program groups is filled up with data."),
                        () => worker.ReportProgress(46, "Status: Table with data about educational programs is filled up with data."),
                        () => worker.ReportProgress(66, "Status: Table with data about courses is filled up with data."),
                        () => worker.ReportProgress(75, "Status: Table with data about allocated lessons is filled up with data.")
                    });

                    worker.ReportProgress(80, "Status: Waiting for connect with server (EFEE)...");

                    EfeeRestService rest = new EfeeRestService(boxes[0].Text, boxes[1].Text, boxes[2].Text);

                    worker.ReportProgress(85, "Status: Sending data to server (EFEE)...");
                    if (rest.insertData(timetable.ToString()))
                    {
                        MessageBox.Show("You successfully post data on server", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        worker.ReportProgress(100, "Status: Posting on server is finished successfully!");
                    }
                    else
                    {
                        MessageBox.Show("You unsuccessfully post data on server", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        worker.ReportProgress(100, "Status: Posting on server is finished unsuccessfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                string mess = "Post on server was not successfull!\nAn error occurred.\n\n";
                mess += ex.Message + "\n" + ex.ToString();
                mess += "\n" + ex.StackTrace;

                MessageBox.Show(mess, "Error");
            }
            finally
            {
                buts.ForEach(elem => elem.button.Enabled = elem.Enabled);
            }
        }

        public static void DoExportXml(BackgroundWorker worker, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "");

            JObject timetable = exportJson(new Action[]
            {
                () => worker.ReportProgress(0, ""),
                () => worker.ReportProgress(3, "Status: Table with data about timetable is filled up."),
                () => worker.ReportProgress(6, "Status: Table with data about days is filled up."),
                () => worker.ReportProgress(12, "Status: Table with data about time periods is filled up."),
                () => worker.ReportProgress(25, "Status: Table with data about teachers is filled up."),
                () => worker.ReportProgress(31, "Status: Table with data about rooms is filled up."),
                () => worker.ReportProgress(37, "Status: Table with data about educational program groups is filled up with data."),
                () => worker.ReportProgress(50, "Status: Table with data about educational programs is filled up with data."),
                () => worker.ReportProgress(75, "Status: Table with data about courses is filled up with data."),
                () => worker.ReportProgress(93, "Status: Table with data about allocated lessons is filled up with data.")
            });

            var xmlJson = new JObject();
            xmlJson.Add("?xml", new JObject
            {
                { "@version", "1.0" },{ "@standalone", "no"}
            });
            xmlJson.Add("root", timetable);
            var xml = JsonConvert.DeserializeXmlNode(xmlJson.ToString());
            StringWriter sw = new StringWriter();
            xml.Save(sw);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\respored.xml", sw.ToString());
            string p = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)  + "\\respored.xml";
            string args = string.Format("/e, /select, \"{0}\"", p);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "explorer";
            info.Arguments = args;
            Process.Start(info);

            worker.ReportProgress(100, "Status: Export to XML file is finished successfully!");
        }
    }
}
