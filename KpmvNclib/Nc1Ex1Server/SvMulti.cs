using System;
using System.Collections.Generic;

namespace Nc1Ex1Server
{
	public class SvMulti : NccpcDll.NccpcNw1Sv
    {
		public List<int> mCs = new List<int>();
        public const int maxP = 2;
        public int?[] mCtis = new int?[maxP];
		public NccpcDll.NccpcMemmgr2Mgr mMm;

		public SvMulti()
			: base()
		{
			mMm = new NccpcDll.NccpcMemmgr2Mgr();
		}

        public bool create()
        { 
            if (!mMm.create()) { return false; }

            var co = new NccpcDll.NccpcNw1Sv.CreateOptions(mMm, "7771");

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
        //public override void onNccpcNwErr(string s1) { qv("Err " + s1); }
        public override void onNccpcNwEnter(int cti, string peer)
        {
            qv("Dbg Multi NwEnter ct:" + cti + " Peer:" + peer);
            mCs.Add(cti);
            int pIndex = -1;
            for (int i = 0; i < maxP; i++)
            {
                qv("index: " + i);
                if (mCtis[i] == null) { mCtis[i] = cti;  pIndex = i; break; }
            }


            using (var pkw = mMm.allocNw1pk(0xff))
            {
                pkw.setType(2);
                pkw.wInt32s(pIndex);
                pkw.wInt32s((int)cti);
                send(cti, pkw);  //send to one
            }
        }

        public override void onNccpcNwRecv(int cti, NccpcDll.NccpcNw1Pk2 ncpk)
        {
            qv("Dbg Multi NwRecv Type:" + ncpk.getType() + " Len:" + ncpk.getDataLen());
            using (var pkw = ncpk.copyDeep())
            {
                send(mCs, pkw);
            }
        }

        public override void onNccpcNwLeave(int cti)
        {
            qv("Dbg Multi  NwLeave ct:" + cti + " remain:" + (mCs.Count - 1));
            for (int i = 0; i < maxP; i++)
            {
                if (mCtis[i] == cti)
                {
                    mCtis[i] = null;
                    qv("Please destroy pidx: " + i);
                    using (var pkw = mMm.allocNw1pk(0xff))
                    {
                        pkw.setType(4);
                        pkw.wInt32s((int)i);
                        send(mCs, pkw);
                    }
                }
            }          
            mCs.Remove(cti);
        }
    }
}
