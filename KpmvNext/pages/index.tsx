import useSWR from 'swr'
import Axios from "axios";
import 'bootstrap/dist/css/bootstrap.css';
import {Carousel, Image} from 'react-bootstrap';
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
            <>
                <div>
                    {image()}
                </div>
                <div>
                    <Adminindex/>
                </div>
            </>
        );
    } else {
        return (
            <>
                <div>
                    {image()}
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

function image() {
    return (
        <div className="d-flex justify-content-center align-items-center">
            <Carousel style={{width: 1200}}>
                <Carousel.Item>
                    <Image
                        className="d-block w-100"
                        src="https://user-images.githubusercontent.com/104874755/224620237-a16af7ef-30ec-4f96-91fe-f48fc31661a3.png"
                        alt="First slide"
                    />
                    <Carousel.Caption>
                        <h3>O/X 퀴즈대회</h3>
                        <p>Nulla vitae elit libero, a pharetra augue mollis interdum.</p>
                    </Carousel.Caption>
                </Carousel.Item>
                <Carousel.Item>
                    <Image
                        className="d-block w-100"
                        src="http://image.ajunews.com//content/image/2021/05/12/20210512155530421254.jpg"
                        alt="Second slide"
                    />
                    <Carousel.Caption>
                        <h3>사고력 테스트 대회</h3>
                        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
                    </Carousel.Caption>
                </Carousel.Item>
                <Carousel.Item>
                    <Image
                        className="d-block w-100"
                        src="http://image.newsis.com/2022/01/28/NISI20220128_0000923157_web.jpg"
                        alt="Third slide"
                    />
                    <Carousel.Caption>
                        <h3>암기력 테스트 대회</h3>
                        <p>
                            Praesent commodo cursus magna, vel scelerisque nisl consectetur.
                        </p>
                    </Carousel.Caption>
                </Carousel.Item>
            </Carousel>
        </div>
    );
}