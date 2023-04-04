import {Tab, Tabs} from "react-bootstrap";
import Quizlist1 from "@/pages/quiz/quizlist1";
import Quiz from "@/pages/quiz/quiz";

function QuizCategory() {
    return (
        <Tabs
            defaultActiveKey="Quiz1"
            className="mb-3"
        >
            <Tab eventKey="Quiz1" title="Quiz1">
                <div>
                    <Quiz/>
                </div>
                <div className="d-flex justify-content-center align-items-center">
                    <Quizlist1/>
                </div>
            </Tab>
            <Tab eventKey="Quiz2" title="Quiz2">
                Quiz2
            </Tab>
            <Tab eventKey="Quiz3" title="Quiz3">
                Quiz3
            </Tab>
            <Tab eventKey="Quiz4" title="another Quiz">
                Quiz4
            </Tab>
            <Tab eventKey="Quiz4" title="another Quiz">
                Quiz4
            </Tab>
        </Tabs>
    );
}

export default QuizCategory;