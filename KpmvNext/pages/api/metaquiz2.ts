import { StatusCodes } from "http-status-codes";
import { NextApiRequest, NextApiResponse } from "next";
import {MongoClient} from 'mongodb';


//db 연결
async function DbConnect1(){
  //const{MongoClient, ServerApiVersion} = require('mongodb');
  const mgct = new MongoClient("mongodb://127.0.0.1:27017");
  const db1 = mgct.db("test");
  const clc1 = db1.collection("quiz2");
  return clc1;
}

// insert
async function DbWrite(content:String, correct:String, answer1:String, answer2:String, answer3:String, answer4:String){
  const clc1 = await DbConnect1();
  const data1 = {
    content: content,
    correct: correct,
    answer1: answer1,
    answer2: answer2,
    answer3: answer3,
    answer4: answer4,
  };
  const result = await clc1.insertOne(data1);
}

//읽기
async function DbRead1(content:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({content:content});
  return data1;
}

//수정
async function DbUpdate1(content:String, answer1:String){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({content:content},{$set:{answer1:answer1}});
}
//삭제
async function DbDelete1(content:String){
  const clc1 = await DbConnect1();
  const result= await clc1.deleteOne({content:content});
}

//전체 읽기
async function DbReadAll(limit = 100){
  const clc1 = await DbConnect1();
  const us = await clc1.find({}).limit(limit);
  return await us.toArray();
}

// eslint-disable-next-line import/no-anonymous-default-export
export default async (req: NextApiRequest, res: NextApiResponse)=>{
  const{add, read, update, del} =req.query;
  
  console.log("usr get add: "+add+" read: "+read);
  
  res.statusCode = StatusCodes.OK;

  if(read){
    return res.send(await DbRead1(String(read)));}
  else if(add){
    await DbWrite(String(add), String(req.query.correct) ,String(req.query.answer1), String(req.query.answer2), String(req.query.answer3), String(req.query.answer4));
    return res.send(await DbReadAll());
  }else if(update){
    await DbUpdate1(String(update), String(req.query.answer));
    return res.send(await DbReadAll());
  }else if(del){
    await DbDelete1(String(del));
    return res.send(await DbReadAll());
  }else{
    let ar1 = await DbReadAll();
    return res.send(JSON.stringify(ar1));
  }
}