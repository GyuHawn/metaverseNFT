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

        public List<BsonDocument> QuizName()
        {
            mQuizName = Mdb1.QuizName();
            foreach (var q1 in mQuizName)
            {
                var s1 = q1.GetValue("game");
                var s2 = q1.GetValue("startTime");
                var s3 = q1.GetValue("winner");
                var s4 = q1.GetValue("quiz");
                Nc1Ex1ServerMainAm2.qv("Dbg mongodb gamename " + s1 + " startTime : " + s2 + " winner : " + s3 + " quiz: " + s4);
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
                if (d1.GetValue("Kind") == "ox")
                {
                    var s1 = d1.GetValue("content");
                    var s2 = d1.GetValue("answer");
                    var s3 = d1.GetValue("explain");
                    var s4 = d1.GetValue("Kind");
                    Nc1Ex1ServerMainAm2.qv("Dbg mongodb OXcontent " + s1 + " : " + s2 + " : " + s3 + " Kind : " + s4);
                }
                else if (d1.GetValue("Kind") == "four")
                {
                    var s1 = d1.GetValue("content");
                    var s2 = d1.GetValue("exp1");
                    var s3 = d1.GetValue("exp2");
                    var s4 = d1.GetValue("exp3");
                    var s5 = d1.GetValue("exp4");
                    var s6 = d1.GetValue("correct");
                    var s7 = d1.GetValue("Kind");
                    Nc1Ex1ServerMainAm2.qv("Dbg mongodb 4content " + s1 + " : " + s2 + " : " + s3 + " : " + s4 + " : " + s5 + " ,정답 : " + s6 + " ,Kind : " + s7);
                }
                else if (d1.GetValue("Kind") == "it")
                {
                    var s1 = d1.GetValue("content");
                    var s2 = d1.GetValue("exp1");
                    var s3 = d1.GetValue("exp2");
                    var s4 = d1.GetValue("exp3");
                    var s5 = d1.GetValue("exp4");
                    var s6 = d1.GetValue("correct");
                    var s7 = d1.GetValue("Kind");
                    Nc1Ex1ServerMainAm2.qv("Dbg mongodb itcontent " + s1 + " : " + s2 + " : " + s3 + " : " + s4 + " : " + s5 + " ,정답 : " + s6 + " ,Kind : " + s7);
                }
            }
            return mQuizList;
        }

        public void QuizDataSend(Nc1Ex1ServerMainAm2.Sv sv, int cti)
        {
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                pkw.setType(100);
                pkw.wInt32s(mQuizList.Count);

                foreach (var d1 in mQuizList)
                {
                    var kind = (string)d1.GetValue("Kind");

                    pkw.wStrToNclib1FromClr(kind);

                    if (kind == "ox")
                    {
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("content"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("answer"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("explain"));
                    }
                    else if (kind == "four" || kind == "it")
                    {
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("content"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("exp1"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("exp2"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("exp3"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("exp4"));
                        pkw.wStrToNclib1FromClr((string)d1.GetValue("correct"));
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