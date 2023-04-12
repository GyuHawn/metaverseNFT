using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nc1Ex1Server
{
    class Nc1Ex1ServerMainAm2
    {
        public class Sv : NccpcDll.NccpcNw1Sv
        {
            public List<int> mCs = new List<int>();
            public NetworkTextTestExample mNtte = new NetworkTextTestExample();
            public NetworkTextTestExample mNttep = new NetworkTextTestExample();
            public NetworkTextTestExample mNtteG = new NetworkTextTestExample();
            public NccpcDll.NccpcMemmgr2Mgr mMm;

            public Sv()
                : base()
            {
                mMm = new NccpcDll.NccpcMemmgr2Mgr();
            }

            public bool create()
            {
                mNtte.Db();
                mNttep.Dbp();
                mNtteG.QuizName();

                if (!mMm.create()) { return false; }

                var co = new NccpcDll.NccpcNw1Sv.CreateOptions(mMm, "7777");

                if (!base.create(co)) { return false; }

                return true;
            }

            new public void release()
            {
                base.release();
                mMm.release();
            }

            public void qv(string s1) { System.Console.WriteLine(s1); }
            public override void onNccpcNwLog(string s1) { qv(s1); }
            public override void onNccpcNwEnter(int cti, string peer)
            {
                qv("Dbg NwEnter ct:" + cti + " Peer:" + peer);
                mCs.Add(cti);
                mNtteG.QuizNameSend(this, cti);
                mNttep.PlayerDataSend(this, cti);
                mNtte.QuizDataSend(this, cti);
            }

            public override void onNccpcNwRecv(int cti, NccpcDll.NccpcNw1Pk2 ncpk)
            {
                qv("Dbg NwRecv Type:" + ncpk.getType() + " Len:" + ncpk.getDataLen());
                if (ncpk.getType() == 111)
                {
                    string s1 = ncpk.rStrFromNclib1ToClr();
                    string s2 = ncpk.rStrFromNclib1ToClr();
                    string s3 = ncpk.rStrFromNclib1ToClr();
                    Mdb1.DbEx_UpdateTest(s1, s2, s3);
                    qv("Dbg NwRecv Type:" + ncpk.getType() + " game : " + s1 + "name : " + s2 + " addr : " + s3);

                    return;
                }
                using (var pkw = ncpk.copyDeep())
                {
                    send(mCs, pkw);
                }
            }
            public override void onNccpcNwLeave(int cti) { qv("Dbg NwLeave ct:" + cti + " remain:" + (mCs.Count - 1)); mCs.Remove(cti); }

        }

        static public void qv(string s1) { System.Console.WriteLine(s1); }

        static void Main(string[] args)
        {
            NccpcDll.NccpcNw1Cmn.stWsaStartup();

            qv("Dbg mongodb rst");

            var sv = new Sv();

            qv("Dbg server starting");

            if (!sv.create()) { return; }

            qv("Dbg server started");

            var svm = new SvMulti();
            qv("Dbg multi server starting");
            if (!svm.create()) { return; }

            bool bWhile = true;

            while (bWhile)
            {
                sv.framemove();
                svm.framemove();

                System.Threading.Thread.Sleep(100);
            }

            sv.release();

            NccpcDll.NccpcNw1Cmn.stWsaCleanup();
        }
    }
}