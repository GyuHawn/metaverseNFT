import {StatusCodes} from "http-status-codes";
import {IronSessionOptions} from "iron-session";
import {NextApiRequest, NextApiResponse} from "next";
import {withIronSessionApiRoute} from "iron-session/next"
import Axios from "axios";


export const sessionOption1: IronSessionOptions = {
    password: 'testpassword1234567890!@#$%^&*()',
    cookieName: 'myservice ironsession testXXX',
}

export class UserData {
    name: string;
    password: string;

    constructor(name: string, password: string) {
        this.name = name;
        this.password = password;
    }
}

declare module 'Iron-session' {
    interface IronSessionData {
        userdata?: UserData
    }
}

export default withIronSessionApiRoute(Dispatch1, sessionOption1);

async function Dispatch1(req: NextApiRequest, res: NextApiResponse) {
    if (req.query.data != undefined) {
        return Data(req, res);
    }
    if (req.query.login != undefined) {
        return await Login(req, res);
    }
    if (req.query.logout != undefined) {
        return Logout(req, res)
    }
    return res.status(StatusCodes.BAD_REQUEST).end();
}

async function Login(req: NextApiRequest, res: NextApiResponse) {
    if (req.session.userdata) {
        req.session.destroy();
    }

    const user = await Axios.get("http://localhost:3000/api/auth?read="+ req.body.Userid+'&password='+req.body.Userpwd).then((data)=>{
        return data.data;
    });

    if(user.userid !== req.body.Userid && req.body.Userpwd !== user.password){
        res.status(401).json({message: '존재하지 않습니다.'});
    }else {
        let ud = new UserData(user.userid, user.password);
        req.session.userdata = ud;
        await req.session.save();
        return res.json({logon: true, userdata: ud});
    }
}

async function Logout(req: NextApiRequest, res: NextApiResponse) {
    req.session.destroy();
    res.json({logon: false, userdata: {}})
}

async function Data(req: NextApiRequest, res: NextApiResponse) {
    if (!req.session.userdata) {
        return res.json({logon: false, userdata: {}});
    }
    return res.json({logon: true, userdata: req.session.userdata});
}