import {StatusCodes} from "http-status-codes";
import {NextApiRequest, NextApiResponse} from "next";
import {MongoClient} from "mongodb";

//db 연결
async function DbConnect1() {
    const mgct = new MongoClient("mongodb://127.0.0.1:27017");
    const db1 = mgct.db("test");
    const clc1 = db1.collection("users");
    return clc1;
}

// 회원가입
async function Dbsignup(userid: string, password: string, username: string, email: string, EOA: string, CA: string, wins: number) {
    const clc1 = await DbConnect1();
    const data1 = {
        userid,
        username,
        email,
        password,
        EOA,
        CA,
        wins,
    };
    const result = await clc1.insertOne(data1);
}

//읽기
async function DbRead1(userid: String, password: String) {
    const clc1 = await DbConnect1();
    const data1 = await clc1.findOne({userid, password});
    return data1;
}

//수정
async function DbUpdate1(userid: String, password: String, email: String) {
    const clc1 = await DbConnect1();
    const result = await clc1.updateOne({userid}, {$set: {password, email}});
}

//삭제
async function DbDelete1(userid: String) {
    const clc1 = await DbConnect1();
    const result = await clc1.deleteOne({userid});
}

//전체 읽기
async function DbReadAll(limit = 100) {
    const clc1 = await DbConnect1();
    const us = await clc1.find({}).limit(limit);
    return await us.toArray();
}

// eslint-disable-next-line import/no-anonymous-default-export
export default async (req: NextApiRequest, res: NextApiResponse) => {
    const {add, read, update, del} = req.query;

    console.log("usr get add: " + add + " read: " + read);

    res.statusCode = StatusCodes.OK;

    if (read) {
        return res.send(await DbRead1(String(read), String(req.query.password)));
    } else if (add) {
        await Dbsignup(String(add), String(req.query.password), String(req.query.username), String(req.query.email), String(req.query.EOA), String(req.query.CA), Number(req.query.wins));
        res.send(await DbReadAll());
    } else if (update) {
        await DbUpdate1(String(update), String(req.query.password), String(req.query.email));
        res.send(await DbReadAll());
    } else if (del) {
        await DbDelete1(String(del));
    } else {
        let ar1 = await DbReadAll();
        return res.send(JSON.stringify(ar1));
    }
}