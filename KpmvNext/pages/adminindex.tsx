import 'bootstrap/dist/css/bootstrap.css';
import {Carousel, Image, Stack} from 'react-bootstrap';
import Time from "@/pages/time";
import Competition from "@/pages/competition";
import Quiz from "@/pages/quiz/quiz";
import Object from "@/pages/object/object";


const App1 = () => {

    return (
        <>
            <div className="m-5">
                <Time/>
            </div>
            <Stack className="d-flex justify-content-center" direction="horizontal" gap={3}>
                <Object/>
                <Quiz/>
                <Competition/>
            </Stack>
            <br/>
        </>
    );
}

export default App1;