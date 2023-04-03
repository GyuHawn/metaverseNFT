import {Button, Stack} from "react-bootstrap";
import {useState} from "react";
import {useRouter} from "next/router";
import Image from 'next/image';
import Axios from "axios";
import useSWR from "swr";

const axios1 = (url: string) => Axios.get(url).then((res) => res.data);

const Gamejoin = () => {
    const {query: qr1} = useRouter();
    const [ipt, setipt] = useState("");
    const sessionData = '/api/session?data';
    const {data, error, isLoading} = useSWR(sessionData, axios1);

    if (error) {
        return <>error!</>;
    }
    if (!data) {
        return <>loading</>;
    }

    return (
        <>
            <div className={"d-flex justify-content-center"}>
                <h1>{qr1.gamejoin}</h1>
            </div>
            <div className={"d-flex justify-content-center"}>
                <h1> 참가자 : {data.userdata.name}</h1>
            </div>
            <div className={"d-flex justify-content-center"}>
                <h1> {ipt === "" ? '캐릭터를 선택해 주세요!!' : ipt + '를 선택하였습니다.'} </h1>
            </div>
            <Stack direction="horizontal" gap={3} className={"d-flex justify-content-center align-items-center"}>
                <div className="bg-light border">
                    <Button variant="light" onClick={() => {
                        setipt("blue")
                    }}>
                        <Image src="/images/Bule.PNG" alt={"tests"} width={387} height={555}/>
                    </Button>
                </div>
                <div className="bg-light border">
                    <Button variant="light" onClick={() => {
                        setipt("red")
                    }}>
                        <Image src="/images/Red.PNG" alt={"tests"} width={387} height={555}/>
                    </Button>
                </div>
                <div className="bg-light border">
                    <Button variant="light" onClick={() => {
                        setipt("white")
                    }}>
                        <Image src="/images/White.PNG" alt={"tests"} width={387} height={555}/>
                    </Button>
                </div>
            </Stack>
            <div>
                <input type="text" value={ipt}/>
            </div>
            <div className={"d-grid gap-2"}>
                {ipt === "" ? <Button size="lg" onClick={() => {
                    alert("캐릭터를 선택해주세요")
                }}>대회 참가</Button> : <Button size="lg" onClick={() => {
                    Axios.get("/api/auth?update2=" + data.userdata.name + "&color=" + ipt);
                    alert("캐릭터 선택완료! " + ipt + "를 선택하셨습니다!");
                }
                }>대회 참가</Button>}
            </div>

            {/*참가자 목록*/}
            <h1>참가자 목록</h1>
        </>
    );
}

export default Gamejoin;