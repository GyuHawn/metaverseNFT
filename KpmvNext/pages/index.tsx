import useSWR from 'swr'
import Axios from "axios";
import 'bootstrap/dist/css/bootstrap.css';
import {Image} from 'react-bootstrap';
import Time from './time';
import Adminindex from "@/pages/adminindex";

const axios1 = (url: string) => Axios.get(url).then((res) => res.data);

const App1 = () => {
    const {data, error, isLoading} = useSWR("/api/session?data", axios1);

    if (error) {
        return (<>error:{JSON.stringify(error)}</>);
    }
    if (!data) {
        return (<> loading data:{data}</>);
    }

    if (data?.userdata?.name === "admin") {
        return (
            <div>
                <Adminindex/>
            </div>
        );
    } else {
        return (
            <>
                <div className="d-flex justify-content-center">
                    <Image
                        src="https://user-images.githubusercontent.com/104874755/224620237-a16af7ef-30ec-4f96-91fe-f48fc31661a3.png"
                        roundedCircle alt="metabusimg"/>
                </div>
                {/* 현재시간 */}
                <div className="m-5">
                    <Time/>
                </div>
            </>
        )
    }
    ;
}
export default App1;