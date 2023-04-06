import { StatusCodes } from "http-status-codes";
import { NextApiRequest, NextApiResponse } from "next";
import {MongoClient} from "mongodb";

//db 연결
async function DbConnect1(){
  const mgct = new MongoClient("mongodb://127.0.0.1:27017");
  const db1 = mgct.db("test");
  const clc1 = db1.collection("nftLists");
  return clc1;
}

// 우승자 등록
async function DbWrite(game:String, nftId:String, winner:String, EOA: String, CA:String, startTime: String, count:Number,  quiz:String){
  const clc1 = await DbConnect1();
  const data1 = {
    game: game,
    nftId: nftId,
    winner: winner,
    EOA: EOA,
    startTime: startTime,
    CA: CA,
    count: count,
    quiz: quiz,
  };
  const result = await clc1.insertOne(data1);
}

// nftId 조회
async function DbRead1(nftId:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({nftId:nftId});
  return data1;
}
async function DbRead2(winner:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.findOne({winner:winner});
  return data1;
}
async function DbReadById(winner:String){
  const clc1 = await DbConnect1();
  const data1 = await clc1.find({winner:winner});
  return await data1.toArray();
}

//수정
async function DbUpdate1(game:String,winner:String, nftId:String, EOA: String){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({game:game},{$set:{nftId:nftId, winner:winner, EOA:EOA}});
}
async function DbUpdate2(game:String, winner:String, EOA:String, count:Number){
  const clc1 = await DbConnect1();
  const result = await clc1.updateOne({game:game},{$set:{winner:winner, EOA:EOA, count:count}});
}
async function DbUpdate3(game:String, nftId:String){
  const clc1 = await DbConnect1();
  const result = clc1.updateOne({game:game},{$set:{nftId:nftId}});
}
async function DbUpdate4(game:String, quiz:String){
  const clc1 = await DbConnect1();
  const result = clc1.updateOne({game:game},{$set:{quiz:quiz}});
} 

//삭제
async function DbDelete1(nftId:String){
  const clc1 = await DbConnect1();
  const result= await clc1.deleteOne({nftId:nftId});
}

//전체 읽기
async function DbReadAll(limit = 100){
  const clc1 = await DbConnect1();
  const us = await clc1.find({}).limit(limit);
  return await us.toArray();
}

// eslint-disable-next-line import/no-anonymous-default-export
export default async (req: NextApiRequest, res: NextApiResponse)=>{
  const{add, read1, read2,read3, update1, update2, update3,update4, del, nftId, winner, EOA} =req.query;
  
  console.log("usr get add: "+add+" read: "+read1+" read2: "+read2+" update1: "+update3+" nftId: "+nftId+" winner: "+winner + "EOA: " + EOA);
  console.log("어디서 나오는거임?1111111111111");
  res.statusCode = StatusCodes.OK;

  if(read1){
    return res.send(await DbRead1(String(read1)));}
  else if(read2){
    return res.send(await DbRead2(String(read2)));
  }else if(read3){
    let ar1 = await DbReadById(String(read3));
    return res.send(JSON.stringify(ar1));
  }
  else if(add){
    await DbWrite(String(add), String(req.query.nftId), String(req.query.winner), String(req.query.EOA), String(req.query.CA), String(req.query.startTime), Number(req.query.count), String(req.query.quiz));
    return res.send(await DbReadAll());
  }else if(update1){
    await DbUpdate1(String(update1),String(req.query.winner), String(req.query.nftId), String(req.query.EOA));
    return res.send(await DbReadAll());
  }else if(update2){
    await DbUpdate2(String(update2),String(req.query.winner), String(req.query.EOA), Number(req.query.count));
    return res.send(await DbReadAll());
  }else if(update3){
    await DbUpdate3(String(update3), String(req.query.nftId));
    console.log("어디서 나오는거임? nft발급완료?");
    return res.send(await DbReadAll());
  }else if(update4){
    await DbUpdate4(String(update4), String(req.query.quiz));
  }
  else if(del){
    await DbDelete1(String(del));
  }else{
    let ar1 = await DbReadAll();
    console.log("어디서 나오는거임?2222");
    return res.send(JSON.stringify(ar1));
  }
}