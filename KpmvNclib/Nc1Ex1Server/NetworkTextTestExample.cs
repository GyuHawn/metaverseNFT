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
        public List<BsonDocument> mQuizList2 = new List<BsonDocument>();
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
                var s1 = d1.GetValue("content");
                var s2 = d1.GetValue("answer");
                var s3 = d1.GetValue("explain");
                Nc1Ex1ServerMainAm2.qv("Dbg mongodb content " + s1 + " : " + s2 + " : " + s3);
            }
            return mQuizList;
        }

        public void QuizDataSend(Nc1Ex1ServerMainAm2.Sv sv, int cti)
        {
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                //Send(pkw, mList);
                pkw.setType(100);
                pkw.wInt32s(mQuizList.Count);
                foreach (var d1 in mQuizList)
                {
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("content"));
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("answer"));
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("explain"));

                    Nc1Ex1ServerMainAm2.qv("quizdata" + (string)d1.GetValue("content") + (string)d1.GetValue("answer") + (string)d1.GetValue("explain"));
                }
                sv.send(cti, pkw);
            }
        }

        public List<BsonDocument> Db2()
        {
            mQuizList2 = Mdb1.Quiz2();
            foreach (var q2 in mQuizList2)
            {
                var s1 = q2.GetValue("content");
                var s2 = q2.GetValue("answer1");
                var s3 = q2.GetValue("answer2");
                var s4 = q2.GetValue("answer3");
                var s5 = q2.GetValue("answer4");
                var s6 = q2.GetValue("correct");
                Nc1Ex1ServerMainAm2.qv("Dbg mongodb content " + s1 + " : " + s2 + " : " + s3 + " : " + s4 + " : " + s5 + " ,정답 : " + s6);
            }
            return mQuizList2;
        }

        public void QuizDataSend2(Nc1Ex1ServerMainAm2.Sv sv, int cti)
        {
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                pkw.setType(99);
                pkw.wInt32s(mQuizList2.Count);
                foreach (var q2 in mQuizList2)
                {
                    pkw.wStrToNclib1FromClr((string)q2.GetValue("content"));
                    pkw.wStrToNclib1FromClr((string)q2.GetValue("answer1"));
                    pkw.wStrToNclib1FromClr((string)q2.GetValue("answer2"));
                    pkw.wStrToNclib1FromClr((string)q2.GetValue("answer3"));
                    pkw.wStrToNclib1FromClr((string)q2.GetValue("answer4"));
                    pkw.wStrToNclib1FromClr((string)q2.GetValue("correct"));

                    Nc1Ex1ServerMainAm2.qv("quizdata2" + (string)q2.GetValue("content") + (string)q2.GetValue("answer1") + (string)q2.GetValue("answer2") + (string)q2.GetValue("answer3") + (string)q2.GetValue("answer4") + "정답:" + (string)q2.GetValue("correct"));
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