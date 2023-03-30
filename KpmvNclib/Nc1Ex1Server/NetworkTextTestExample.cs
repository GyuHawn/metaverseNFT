﻿using MongoDB.Bson;
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
                Nc1Ex1ServerMainAm2.qv("Dbg mongodb gamename " + s1 + " startTime : " + s2 + " winner : " + s3);
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
                Nc1Ex1ServerMainAm2.qv("Dbg mongodb content " + s1 + " : " + s2);
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

                    Nc1Ex1ServerMainAm2.qv("quizdata" + (string)d1.GetValue("content") + (string)d1.GetValue("answer"));
                }
                sv.send(cti, pkw);
            }
        }

        public List<BsonDocument> Dbp()
        {
            mPlayerList = Mdb1.DbEx_FindPlayer();
            foreach (var d1 in mPlayerList)
            {
                var s1 = d1.GetValue("name");
                var s2 = d1.GetValue("nftAddr");
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
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("name"));
                    pkw.wStrToNclib1FromClr((string)d1.GetValue("nftAddr"));
                }
                sv.send(cti, pkw);
            }
        }

    }
}