import {Tab, Tabs} from "react-bootstrap";
import Objectlist from "@/pages/object/objectlist1";


function QuizCategory() {
    return (
        <Tabs
            defaultActiveKey="Quiz1"
            className="mb-3"
        >
            <Tab eventKey="Quiz1" title="배경리스트">
                <div className="m-lg-5">
                    <Objectlist/>
                </div>
            </Tab>
            <Tab eventKey="Quiz2" title="오브젝트리스트">
                Quiz2
            </Tab>
            <Tab eventKey="Quiz3" title="맵">
                Quiz3
            </Tab>
            <Tab eventKey="Quiz4" title="another Objcet">
                Quiz4
            </Tab>
        </Tabs>
    );
}

export default QuizCategory;