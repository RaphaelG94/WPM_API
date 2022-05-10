//using System;
//using System.Linq;
////using bsshared.database;
////using bsshared;
////using System.Data.Entity;
////using Microsoft.WindowsAzure.Storage.File;
//using System.IO;
//using WPM_API.SmartDeploy.Models;
//using System.Collections.Generic;

//namespace  WPM_API.SmartDeploy_dep
//{
//    public static class SmartDeploy
//    {
//        public static string GetTaskList(string _sn)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    var tasks = (from e in _db.v_getClientTaskList
//                                 where e.client_uid == _sn
//                                 select e).ToList<v_getClientTaskList>();
//                    return (string)bsshared.bsxml._getXml(tasks, typeof(System.Xml.XmlNode), true);
//                }
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }

//        public static string GetClientSoftwareList(string _sn)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {

//                    var sw = _db.v_getClientSoftwareList.Where(p => p.client_uid == _sn).ToList();
//                    return (string)bsshared.bsxml._getXml(sw, typeof(System.Xml.XmlNode), true);
//                }
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }

//        public static List<v_getSoftwareList> GetSoftwareList()
//        {
//            using (var _db = new smartdeploy_db())
//            {
//                return _db.v_getSoftwareList.ToList();
//            }
//        }

//        public static string GetSoftwareListXml()
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {

//                    var sw = _db.v_getSoftwareList.ToList();
//                    return (string)bsshared.bsxml._getXml(sw, typeof(System.Xml.XmlNode), true);
//                }
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }

//        public static string GetRuleDetails(string _rule_id)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    var rule = _db.v_getRuleDetails.Single(p => p.rule_id.ToString() == _rule_id);
//                    return (string)bsshared.bsxml._getXml(rule, typeof(System.Xml.XmlNode), true);
//                }
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }

//        public static string GetTaskDetails(string _sn, string _tuid)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    //_db.Database.Connection.Open();
//                    _db.Configuration.ProxyCreationEnabled = false;
//                    var tasks = (from e in _db.dat_task
//                                 where e.C_uid == _tuid
//                                 select e).ToList<dat_task>();
//                    return (string)bsshared.bsxml._getXml(tasks, typeof(System.Xml.XmlNode), true);
//                }
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }

//        public static MemoryStream GetFile(string _fn, string _fp)
//        {
//            try
//            {
//                var cf = bsstorage.DownloadFile(_fp, _fn);
//                var ms = new System.IO.MemoryStream();
//                cf.DownloadToStreamAsync(ms);
//                ms.Seek(0, System.IO.SeekOrigin.Begin);
//                return ms;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public static MemoryStream GetSoftwareIcon(string _sw_id)
//        {
//            try
//            {
//                string _fp = "sw_icons";
//                string _fn = _sw_id + ".ico";
//                var cf = bsstorage.DownloadFile(_fp, _fn);
//                var ms = new MemoryStream();
//                cf.DownloadToStreamAsync(ms);
//                ms.Seek(0, SeekOrigin.Begin);
//                return ms;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public static string GetFileList(string _tuid)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    //_db.Database.Connection.Open();
//                    _db.Configuration.ProxyCreationEnabled = false;
//                    //var files = _db.lnk_taskfile.Where(p => p.dat_task.C_uid == _tuid).Select(o => o.dat_file).ToList();
//                    //var files = _db.dat_task.Where(p => p.C_uid == _tuid).Select(o => o.dat_file).ToList();
//                    var files = _db.v_getTaskFiles.Where(p => p.task_uid == _tuid).ToList();
//                    //var files = _db.dat_task.Where(p => p.C_uid == _tuid).Single().dat_file.ToList();
//                    return (string)bsshared.bsxml._getXml(files, typeof(System.Xml.XmlNode), true);
//                }
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public static async System.Threading.Tasks.Task<string> GetFilesInDirectoryAsync(string _dirPath)
//        {
//            try
//            {
//                var listing = await bsstorage.GetFileListingAsync(_dirPath);
//                string retS = String.Empty;
//                foreach (var s in listing)
//                {
//                    retS += s + ",";
//                }
//                retS = retS.TrimEnd(',');
//                return retS;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }

//        public static async System.Threading.Tasks.Task<string> GetDirsInDirectoryAsync(string _dirPath)
//        {
//            try
//            {
//                var listing = await bsstorage.GetDirListingAsync(_dirPath);
//                string retS = String.Empty;
//                foreach (var s in listing)
//                {
//                    retS += s + ",";
//                }
//                retS = retS.TrimEnd(',');
//                return retS;
//            }
//            catch (Exception ex)
//            {
//                return null;
//            }
//        }


//        public static string SetTaskStatus(string _ctid, string _status, string _message)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    // _db.Database.Connection.Open();
//                    bool goterror = false;
//                    dat_status ds = new dat_status();
//                    ds.status = _status;
//                    ds.message = _message;
//                    var lnk_ct = _db.lnk_clienttask.Single(p => p.C_id.ToString() == _ctid);
//                    ds.lnk_clienttask = lnk_ct;
//                    ds.C_date = System.DateTime.Now;
//                    _db.dat_status.Add(ds);
//                    _db.SaveChanges();
//                    //check if final state
//                    //right now check for status=="executed" & message = return code: 0
//                    if (_status.Trim().ToLower() == "executed" & _message.Trim().ToLower() == "return code: 0")
//                    {
//                        //grab all stati where lnk_clienttask id and move to status_history table
//                        var stats = _db.dat_status.Where(p => p.fk_lnk_clienttask_id == lnk_ct.C_id).ToList();
//                        foreach (var stat in stats)
//                        {
//                            var sh = new dat_status_history();
//                            sh.C_date = stat.C_date;
//                            sh.lnk_clienttask = stat.lnk_clienttask;
//                            sh.message = stat.message;
//                            sh.status = stat.status;
//                            _db.dat_status_history.Add(sh);
//                            _db.dat_status.Remove(stat);
//                        }
//                        _db.SaveChanges();
//                    }
//                }

//                return "true";
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }

//        public static string AssignTask(string _tid, string _cuid)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    //do only if task-client assignment is not active
//                    if (!_db.dat_status.Any(p => p.lnk_clienttask.dat_client.C_uid.ToString() == _cuid & p.lnk_clienttask.fk_dat_task_id.ToString() == _tid))
//                    {
//                        bool goterror = false;
//                        var lnkct = new lnk_clienttask();
//                        lnkct.dat_client = _db.dat_client.Single(p => p.C_uid.ToString() == _cuid);
//                        lnkct.dat_task = _db.dat_task.Single(p => p.C_id.ToString() == _tid);
//                        _db.lnk_clienttask.Add(lnkct);
//                        _db.SaveChanges();
//                        var stat = new dat_status();
//                        stat.status = "assigned";
//                        stat.message = System.DateTime.Now.ToString();
//                        stat.C_date = System.DateTime.Now;
//                        lnkct.dat_status.Add(stat);
//                        _db.SaveChanges();
//                    }
//                }

//                return "true";
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }

//        public static string AssignSoftware(string _sw_id, string _cuid)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    //do only if task-client assignment is not active
//                    var c = _db.dat_client.Single(p => p.C_uid == _cuid);
//                    var sw = _db.dat_software.Single(p => p.C_id.ToString() == _sw_id);
//                    c.dat_software.Add(sw);
//                    //var lnk = new lnk_clientsoftware();
//                    //lnk.dat_client = c;
//                    //lnk.dat_software = sw;
//                    _db.SaveChanges();
//                }

//                return "true";
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }

//        public static string UnassignSoftware(string _sw_id, string _cuid)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    //do only if task-client assignment is not active
//                    var c = _db.dat_client.Single(p => p.C_uid == _cuid);
//                    var sw = _db.dat_software.Single(p => p.C_id.ToString() == _sw_id);
//                    //var l_lnk = _db.lnk_clientsoftware.Where(p => p.fk_dat_software_id.ToString()==_sw_id & p.dat_client.C_uid==_cuid);
//                    //foreach(var lnk in l_lnk) {
//                    //    _db.lnk_clientsoftware.Remove(lnk);
//                    //}
//                    var clients = sw.dat_client.Where(p => p.C_uid == _cuid);
//                    foreach (var client in clients)
//                        sw.dat_client.Remove(client);
//                    _db.SaveChanges();
//                    var _tid = sw.dat_task_uninst.C_id.ToString();
//                    if (!_db.dat_status.Any(p => p.lnk_clienttask.dat_client.C_uid.ToString() == _cuid & p.lnk_clienttask.fk_dat_task_id.ToString() == _tid))
//                    {
//                        bool goterror = false;
//                        var lnkct = new lnk_clienttask();
//                        lnkct.dat_client = _db.dat_client.Single(p => p.C_uid.ToString() == _cuid);
//                        lnkct.dat_task = _db.dat_task.Single(p => p.C_id.ToString() == _tid);
//                        _db.lnk_clienttask.Add(lnkct);
//                        _db.SaveChanges();
//                        var stat = new dat_status();
//                        stat.status = "assigned";
//                        stat.message = System.DateTime.Now.ToString();
//                        stat.C_date = System.DateTime.Now;
//                        lnkct.dat_status.Add(stat);
//                        _db.SaveChanges();
//                    }
//                }

//                return "true";
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }



//        public static string SetAssignTask2Client(string _tid, string _cid)
//        {
//            try
//            {
//                using (var _db = new smartdeploy_db())
//                {
//                    // _db.Database.Connection.Open();
//                    bool goterror = false;
//                    var lnkct = new lnk_clienttask();
//                    lnkct.dat_client = _db.dat_client.Single(p => p.C_id.ToString() == _cid);
//                    lnkct.dat_task = _db.dat_task.Single(p => p.C_id.ToString() == _tid);
//                    _db.lnk_clienttask.Add(lnkct);
//                    _db.SaveChanges();
//                    var stat = new dat_status();
//                    stat.status = "assigned";
//                    stat.message = System.DateTime.Now.ToString();
//                    stat.C_date = System.DateTime.Now;
//                    lnkct.dat_status.Add(stat);
//                    _db.SaveChanges();
//                }

//                return "true";
//            }
//            catch (Exception ex)
//            {
//                return "Exception: " + ex.Message;
//            }
//        }
//    }
//}
