import {Tab, Tabs} from "react-bootstrap";
import Quizlist1 from "@/pages/quiz/quizlist1";
import Quizlist2 from "@/pages/quiz/quizlist2";
import Choice from "@/pages/quiz/choice";
function QuizCategory() {
    return (
        <Tabs
            defaultActiveKey="Quiz1"
            className="mb-3"
        >
            <Tab eventKey="Quiz1" title="OX 퀴즈">
                <div className="m-lg-5">
                    <Quizlist1/>
                    <Choice/>
                </div>
            </Tab>
            <Tab eventKey="Quiz2" title="4지선다형">
                <div className="m-lg-6">
                    <Quizlist2/>
                    <Choice/>
                </div>
            </Tab>
            <Tab eventKey="Quiz3" title="수학문제">
                Quiz3
            </Tab>
            <Tab eventKey="Quiz4" title="another Quiz">
                Quiz4
            </Tab>
        </Tabs>
    );
}

export default QuizCategory;