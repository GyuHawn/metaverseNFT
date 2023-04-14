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
async function DbWrite(content:String, correct:String, explain:String, Kind:String){
  const clc1 = await DbConnect1();
  const data1 = {
    content: content,
    correct: correct,
    explain: explain,
    Kind: Kind,
  };
  const result = await clc1.insertOne(data1);
}
//4지선다형
async function DbWrite2(content:String, correct:String, exp1:String, exp2:String, exp3:String, exp4:String, Kind:String){
  const clc1 = await DbConnect1();
  const data1 = {
    content: content,
    correct: correct,
    exp1: exp1,
    exp2: exp2,
    exp3: exp3,
    exp4: exp4,
    Kind: Kind,
  };
  const result = await clc1.insertOne(data1);
}
//it문제
async function DbWrite3(content:String, correct:String, exp1:String, exp2:String, exp3:String, exp4:String, Kind:String){
  const clc1 = await DbConnect1();
  const data1 = {
    content: content,
    correct: correct,
    exp1: exp1,
    exp2: exp2,
    exp3: exp3,
    exp4: exp4,
    Kind: Kind,
  };
  const result = await clc1.insertOne(data1);
}
//==============================================================================================================================
//읽기 OX
async function DbRead1(content:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({content:content});
  return data1;
}
//읽기 4지선다
async function DbRead2(content:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({content:content});
  return data1;
}
//읽기 it문제
async function DbRead3(content:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({content:content});
  return data1;
}

//==============================================================================================================================
//수정 OX
async function DbUpdate1(content:String, correct:String, explain:String){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({content:content},{$set:{correct, explain}});
}
//수정 4지선다
async function DbUpdate2(content:String, correct:String, exp1:String, exp2:String, exp3:String, exp4:String){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({content:content},{$set:{correct, exp1, exp2, exp3, exp4 }});
}
//수정 it문제
async function DbUpdate3(content:String, correct:String, exp1:String, exp2:String, exp3:String, exp4:String){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({content:content},{$set:{correct, exp1, exp2, exp3, exp4 }});
}
//==============================================================================================================================
//삭제 OX
async function DbDelete1(content:String){
  const clc1 = await DbConnect1();
  const result= await clc1.deleteOne({content:content});
}
//삭제 4지선다
async function DbDelete2(content:String){
  const clc1 = await DbConnect1();
  const result= await clc1.deleteOne({content:content});
}
//삭제 it문제
async function DbDelete3(content:String){
  const clc1 = await DbConnect1();
  const result= await clc1.deleteOne({content:content});
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
    await DbWrite(String(add),String(req.query.correct), String(req.query.explain), String(req.query.Kind));
    return res.send(await DbReadAll());
  }else if(add2){
    await DbWrite2(String(add2), String(req.query.correct) ,String(req.query.exp1), String(req.query.exp2), String(req.query.exp3), String(req.query.exp4), String(req.query.Kind));
    return res.send(await DbReadAll());
  }else if(add3){
    await DbWrite3(String(add3), String(req.query.correct) ,String(req.query.exp1), String(req.query.exp2), String(req.query.exp3), String(req.query.exp4), String(req.query.Kind));
    return res.send(await DbReadAll());
  }else if(update){
    await DbUpdate1(String(update), String(req.query.correct), String(req.query.explain));
    return res.send(await DbReadAll());
  }else if(update2){
    await DbUpdate2(String(update2), String(req.query.correct), String(req.query.exp1), String(req.query.exp2), String(req.query.exp3), String(req.query.exp4));
    return res.send(await DbReadAll());
  }else if(update3){
    await DbUpdate3(String(update3), String(req.query.correct), String(req.query.exp1), String(req.query.exp2), String(req.query.exp3), String(req.query.exp4));
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