import { StatusCodes } from "http-status-codes";
import { NextApiRequest, NextApiResponse } from "next";
import {MongoClient} from 'mongodb';

//db 연결
async function DbConnect1(){
  const mgct = new MongoClient("mongodb://127.0.0.1:27017");
  const db1 = mgct.db("test");
  const clc1 = db1.collection("quiz");
  return clc1;
}
//==============================================================================================================================
// OX퀴즈
async function DbWrite(OXcontent:String, OXanswer:String, OXexplain:String, Kind:String){
  const clc1 = await DbConnect1();
  const data1 = {
    OXcontent: OXcontent,
    OXanswer: OXanswer,
    OXexplain: OXexplain,
    Kind: Kind,
  };
  const result = await clc1.insertOne(data1);
}
//4지선다형
async function DbWrite2(Fcontent:String, Fcorrect:String, Fexp1:String, Fexp2:String, Fexp3:String, Fexp4:String, Kind:String){
  const clc1 = await DbConnect1();
  const data1 = {
    Fcontent: Fcontent,
    Fcorrect: Fcorrect,
    Fexp1: Fexp1,
    Fexp2: Fexp2,
    Fexp3: Fexp3,
    Fexp4: Fexp4,
    Kind: Kind,
  };
  const result = await clc1.insertOne(data1);
}
//it문제
async function DbWrite3(ITcontent:String, ITcorrect:String, ITexp1:String, ITexp2:String, ITexp3:String, ITexp4:String, Kind:String){
  const clc1 = await DbConnect1();
  const data1 = {
    ITcontent: ITcontent,
    ITcorrect: ITcorrect,
    ITexp1: ITexp1,
    ITexp2: ITexp2,
    ITexp3: ITexp3,
    ITexp4: ITexp4,
    Kind: Kind,
  };
  const result = await clc1.insertOne(data1);
}
//==============================================================================================================================
//읽기 OX
async function DbRead1(OXcontent:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({OXcontent:OXcontent});
  return data1;
}
//읽기 4지선다
async function DbRead2(Fcontent:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({Fcontent:Fcontent});
  return data1;
}
//읽기 it문제
async function DbRead3(ITcontent:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({ITcontent:ITcontent});
  return data1;
}

//==============================================================================================================================
//수정 OX
async function DbUpdate1(OXcontent:String, OXanswer:String, OXexplain:String){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({OXcontent:OXcontent},{$set:{OXanswer, OXexplain}});
}
//수정 4지선다
async function DbUpdate2(Fcontent:String, Fcorrect:String, Fexp1:String, Fexp2:String, Fexp3:String, Fexp4:String){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({Fcontent:Fcontent},{$set:{Fcorrect, Fexp1, Fexp2, Fexp3, Fexp4 }});
}
//수정 it문제
async function DbUpdate3(ITcontent:String, ITcorrect:String, ITexp1:String, ITexp2:String, ITexp3:String, ITexp4:String){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({ITcontent:ITcontent},{$set:{ITcorrect, ITexp1, ITexp2, ITexp3, ITexp4 }});
}
//==============================================================================================================================
//삭제 OX
async function DbDelete1(OXcontent:String){
  const clc1 = await DbConnect1();
  const result= await clc1.deleteOne({OXcontent:OXcontent});
}
//삭제 4지선다
async function DbDelete2(Fcontent:String){
  const clc1 = await DbConnect1();
  const result= await clc1.deleteOne({Fcontent:Fcontent});
}
//삭제 it문제
async function DbDelete3(ITcontent:String){
  const clc1 = await DbConnect1();
  const result= await clc1.deleteOne({ITcontent:ITcontent});
}
//==============================================================================================================================
//전체 읽기
async function DbReadAll(limit = 100){
  const clc1 = await DbConnect1();
  const us = await clc1.find({}).limit(limit);
  return await us.toArray();
}

//==============================================================================================================================
export default async (req: NextApiRequest, res: NextApiResponse)=>{
  const{add,add2,add3, read,read2,read3, update, update2,update3, del, del2, del3} =req.query;
  
  console.log("usr get add: "+add+" read: "+read);
  
  res.statusCode = StatusCodes.OK;

  if(read){
    return res.send(await DbRead1(String(read)));
  }else if(read2){
    return res.send(await DbRead2(String(read2)));
  }else if(read3){
    return res.send(await DbRead3(String(read3)));
  }else if(add){
    await DbWrite(String(add),String(req.query.OXanswer), String(req.query.OXexplain), String(req.query.Kind));
    return res.send(await DbReadAll());
  }else if(add2){
    await DbWrite2(String(add2), String(req.query.Fcorrect) ,String(req.query.Fexp1), String(req.query.Fexp2), String(req.query.Fexp3), String(req.query.Fexp4), String(req.query.Kind));
    return res.send(await DbReadAll());
  }else if(add3){
    await DbWrite3(String(add3), String(req.query.ITcorrect) ,String(req.query.ITexp1), String(req.query.ITexp2), String(req.query.ITexp3), String(req.query.ITexp4), String(req.query.Kind));
    return res.send(await DbReadAll());
  }else if(update){
    await DbUpdate1(String(update), String(req.query.OXanswer), String(req.query.OXexplain));
    return res.send(await DbReadAll());
  }else if(update2){
    await DbUpdate2(String(update2), String(req.query.Fcorrect), String(req.query.Fexp1), String(req.query.Fexp2), String(req.query.Fexp3), String(req.query.Fexp4));
    return res.send(await DbReadAll());
  }else if(update3){
    await DbUpdate3(String(update3), String(req.query.ITcorrect), String(req.query.ITexp1), String(req.query.ITexp2), String(req.query.ITexp3), String(req.query.ITexp4));
    return res.send(await DbReadAll());
  }else if(del){
    await DbDelete1(String(del));
    return res.send(await DbReadAll());
  }else if(del2){
    await DbDelete2(String(del2));
    return res.send(await DbReadAll());
  }else if(del3){
    await DbDelete3(String(del3));
    return res.send(await DbReadAll());
  }else{
    let ar1 = await DbReadAll();
    return res.send(JSON.stringify(ar1));
  }
}