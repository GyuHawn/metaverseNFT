using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nc1Ex1Server
{
    class NetworkTextTestExample
    {
        public List<BsonDocument> mQuizList = new List<BsonDocument>();
        public List<BsonDocument> mPlayerList = new List<BsonDocument>();
        public List<BsonDocument> mQuizName = new List<BsonDocument>();
        string kind;

        public List<BsonDocument> QuizName()
        {
            mQuizName = Mdb1.QuizName();
            foreach (var q1 in mQuizName)
            {
                if (q1.GetValue("winner") == "")
                {
                    kind = (string)q1.GetValue("quiz");
                }
                var s1 = q1.GetValue("game");
                var s2 = q1.GetValue("startTime");
                var s3 = q1.GetValue("winner");
                Nc1Ex1ServerMainAm2.qv("Dbg mongodb gamename " + s1 + " startTime : " + s2 + " winner : " + s3 + " quiz: " + kind);
            }
            return mQuizName;
        }

        public void QuizNameSend(Nc1Ex1ServerMainAm2.Sv sv, int cti)
        {
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                pkw.setType(102);
                pkw.wInt32s(mQuizName.Count);
                foreach (var q1 in mQuizName)
                {
                    pkw.wStrToNclib1FromClr((string)q1.GetValue("game"));
                    pkw.wStrToNclib1FromClr((string)q1.GetValue("startTime"));
                    pkw.wStrToNclib1FromClr((string)q1.GetValue("winner"));
                    pkw.wStrToNclib1FromClr((string)q1.GetValue("quiz"));
                }
                sv.send(cti, pkw);
            }
        }

        public List<BsonDocument> Db()
        {

            mQuizList = Mdb1.DbEx_FindAll();

            foreach (var d1 in mQuizList)
            {
                if (kind == "ox")
                {
                    var s1 = d1.GetValue("content");
                    var s2 = d1.GetValue("correct");
                    var s3 = d1.GetValue("Kind");
                    var s4 = d1.GetValue("explain");
                    Nc1Ex1ServerMainAm2.qv("Dbg mongodb content " + s1 + ", correct : " + s2 + ", Kind " + s3 + ", explain : " + s4);
                }
                else if (kind == "four" || kind == "it")
                {
                    var s1 = d1.GetValue("content");
                    var s2 = d1.GetValue("correct");
                    var s3 = d1.GetValue("Kind");
                    var s4 = d1.GetValue("exp1");
                    var s5 = d1.GetValue("exp2");
                    var s6 = d1.GetValue("exp3");
                    var s7 = d1.GetValue("exp4");
                    Nc1Ex1ServerMainAm2.qv("Dbg mongodb content " + s1 + ", correct : " + s2 + " : " + s4 + " : " + s5 + " : " + s6 + " : " + s7 + ", Kind : " + s3);
                }
            }
            return mQuizList;
        }

        public void QuizDataSend(Nc1Ex1ServerMainAm2.Sv sv, int cti)
        {
            var qzname = ""; //<-대회정보
            foreach (var q1 in mQuizName)
            {
                if ((string)q1.GetValue("winner") == "")
                {
                    qzname = (string)q1.GetValue("quiz");
                }
            }
            var qzs = mQuizList.FindAll(x => x["Kind"] == qzname);
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                pkw.setType(100);
                pkw.wInt32s(qzs.Count);

                foreach (var d1 in qzs)
                {
                    var kind = (string)d1.GetValue("Kind");
                    pkw.wStrToNclib1FromClr(kind);
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("content"));
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("correct"));
                    if (kind == "ox")
                    {
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("explain"));
                    }
                    if (kind == "four" || kind == "it")
                    {
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("exp1"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("exp2"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("exp3"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("exp4"));
                    }
                }
                sv.send(cti, pkw);
            }
        }

        public List<BsonDocument> Dbp()
        {
            mPlayerList = Mdb1.DbEx_FindPlayer();
            foreach (var d1 in mPlayerList)
            {
                var s1 = d1.GetValue("username");
                var s2 = d1.GetValue("EOA");
                var s3 = d1.GetValue("userid");
                var s4 = d1.GetValue("password");
                var s5 = d1.GetValue("color");
                Nc1Ex1ServerMainAm2.qv("Dbg mongodb name " + s1 + " nftAddr : " + s2);
            }
            return mPlayerList;
        }

        public void PlayerDataSend(Nc1Ex1ServerMainAm2.Sv sv, int cti)
        {
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                pkw.setType(101);
                pkw.wInt32s(mPlayerList.Count);
                foreach (var d1 in mPlayerList)
                {
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("username"));
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("EOA"));
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("userid"));
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("password"));
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("color"));
                }
                sv.send(cti, pkw);
            }
        }

    }
}