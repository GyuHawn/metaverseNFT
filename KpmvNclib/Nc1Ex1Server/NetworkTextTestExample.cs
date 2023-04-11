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
        public List<oxQuiz> mQuizList = new List<oxQuiz>(); //OX문제
        public List<fQuiz> mQuizList2 = new List<fQuiz>(); //4지선다
        public List<itQuiz> mQuizList3 = new List<itQuiz>(); //IT문제
        public List<BsonDocument> mPlayerList = new List<BsonDocument>();
        public List<BsonDocument> mQuizName = new List<BsonDocument>();

        public class oxQuiz
        {
            public string oxContent, oxAnswer, oxExplain, oxKind;
        }

        public class fQuiz
        {
            public string fContent, fEx1, fEx2, fEx3, fEx4, fCorrect, fKind;
        }

        public class itQuiz
        {
            public string itContent, itEx1, itEx2, itEx3, itEx4, itCorrect, itKind;
        }

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

        public List<oxQuiz> Db() //OX퀴즈
        {
            var quizList = Mdb1.DbEx_FindAll();
            foreach (var d1 in quizList)
            {
                if (d1.Contains("OXcontent"))
                {
                    oxQuiz oxlist = new oxQuiz();
                    var s1 = (string)d1.GetValue("OXcontent");
                    var s2 = (string)d1.GetValue("OXanswer");
                    var s3 = (string)d1.GetValue("OXexplain");
                    var s4 = (string)d1.GetValue("Kind");
                    Nc1Ex1ServerMainAm2.qv("Dbg mongodb OXcontent " + s1 + " : " + s2 + " : " + s3 + " Kind : " + s4);

                    oxlist.oxContent = s1;
                    oxlist.oxAnswer = s2;
                    oxlist.oxExplain = s3;
                    oxlist.oxKind = s4;

                    mQuizList.Add(oxlist);
                }
            }
            return mQuizList;
        }

        public void QuizDataSend(Nc1Ex1ServerMainAm2.Sv sv, int cti) //OX퀴즈
        {
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                //Send(pkw, mList);
                pkw.setType(100);
                pkw.wInt32s(mQuizList.Count);
                foreach (var d1 in mQuizList)
                {
                    pkw.wStrToNclib1FromClr(d1.oxContent);
                    pkw.wStrToNclib1FromClr(d1.oxAnswer);
                    pkw.wStrToNclib1FromClr(d1.oxExplain);
                    pkw.wStrToNclib1FromClr(d1.oxKind);
                }
                sv.send(cti, pkw);
            }
        }

        public List<fQuiz> Db2() //4지선다
        {
            var quizList2 = Mdb1.Quiz2();

            foreach (var q2 in quizList2)
            {
                if (q2.Contains("Fcontent"))
                {
                    fQuiz flist = new fQuiz();
                    var s1 = (string)q2.GetValue("Fcontent");
                    var s2 = (string)q2.GetValue("Fexp1");
                    var s3 = (string)q2.GetValue("Fexp2");
                    var s4 = (string)q2.GetValue("Fexp3");
                    var s5 = (string)q2.GetValue("Fexp4");
                    var s6 = (string)q2.GetValue("Fcorrect");
                    var s7 = (string)q2.GetValue("Kind");
                    Nc1Ex1ServerMainAm2.qv("Dbg mongodb 4content " + s1 + " : " + s2 + " : " + s3 + " : " + s4 + " : " + s5 + " ,정답 : " + s6 + " ,Kind : " + s7);

                    flist.fContent = s1;
                    flist.fEx1 = s2;
                    flist.fEx2 = s3;
                    flist.fEx3 = s4;
                    flist.fEx4 = s5;
                    flist.fCorrect = s6;
                    flist.fKind = s7;
                    mQuizList2.Add(flist);
                }
            }
            return mQuizList2;
        }

        public void QuizDataSend2(Nc1Ex1ServerMainAm2.Sv sv, int cti) //4지선다
        {
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                pkw.setType(99);
                //pkw.setType(100);
                pkw.wInt32s(mQuizList2.Count);
                foreach (var q2 in mQuizList2)
                {
                    pkw.wStrToNclib1FromClr(q2.fContent);
                    pkw.wStrToNclib1FromClr(q2.fEx1);
                    pkw.wStrToNclib1FromClr(q2.fEx2);
                    pkw.wStrToNclib1FromClr(q2.fEx3);
                    pkw.wStrToNclib1FromClr(q2.fEx4);
                    pkw.wStrToNclib1FromClr(q2.fCorrect);
                    pkw.wStrToNclib1FromClr(q2.fKind);
                }
                sv.send(cti, pkw);
            }
        }

        public List<itQuiz> Db3() //IT문제
        {
            
            var mquizList3 = Mdb1.Quiz3();
            foreach (var q2 in mquizList3)
            {
                if (q2.Contains("ITcontent"))
                {
                    itQuiz itlist = new itQuiz();
                    var s1 = (string)q2.GetValue("ITcontent");
                    var s2 = (string)q2.GetValue("ITexp1");
                    var s3 = (string)q2.GetValue("ITexp2");
                    var s4 = (string)q2.GetValue("ITexp3");
                    var s5 = (string)q2.GetValue("ITexp4");
                    var s6 = (string)q2.GetValue("ITcorrect");
                    var s7 = (string)q2.GetValue("Kind");
                    Nc1Ex1ServerMainAm2.qv("Dbg mongodb ITcontent " + s1 + " : " + s2 + " : " + s3 + " : " + s4 + " : " + s5 + " ,정답 : " + s6 + " ,kind : " + s7);

                    itlist.itContent = s1;
                    itlist.itEx1 = s2;
                    itlist.itEx2 = s3;
                    itlist.itEx3 = s4;
                    itlist.itEx4 = s5;
                    itlist.itCorrect = s6;
                    itlist.itKind = s7;
                    mQuizList3.Add(itlist);
                }
            }
            return mQuizList3;
        }

        public void QuizDataSend3(Nc1Ex1ServerMainAm2.Sv sv, int cti) //IT문제
        {
            using (var pkw = sv.mMm.allocNw1pk(0xff))
            {
                pkw.setType(98);
                //pkw.setType(100);
                pkw.wInt32s(mQuizList3.Count);
                foreach (var q2 in mQuizList3)
                {
                    pkw.wStrToNclib1FromClr(q2.itContent);
                    pkw.wStrToNclib1FromClr(q2.itEx1);
                    pkw.wStrToNclib1FromClr(q2.itEx2);
                    pkw.wStrToNclib1FromClr(q2.itEx3);
                    pkw.wStrToNclib1FromClr(q2.itEx4);
                    pkw.wStrToNclib1FromClr(q2.itCorrect);
                    pkw.wStrToNclib1FromClr(q2.itKind);
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