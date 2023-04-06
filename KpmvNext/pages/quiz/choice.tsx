import useSWR from 'swr'
import Axios from "axios";
import {useState} from 'react';

const axios1 = (url: string) => Axios.get(url).then((res) => res.data);

const QuizChoice =()=>{
    const {data, error, isLoading} = useSWR("http://localhost:3000/api/nftlist", axios1);

    const [ipt1, setipt1] = useState("");

    if (error) {
        return <>error!</>;
    }
    if (!data) {
        return <>loading</>;
    }
    const emptyWinners = data.filter((e: { winner: string}) => !e.winner);
    return (
        <div>
        {emptyWinners?.map((e: { game: string}) => {
            return <>
                <h1>현재 퀴즈 대회 : {e.game}</h1>
            </>
        })}
        </div>
    );
}
export default QuizChoice;