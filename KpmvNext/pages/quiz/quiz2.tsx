import useSWR from 'swr'
import Axios from "axios";
import { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import {Button, Modal, Dropdown, Form} from 'react-bootstrap';

export default function Quiz2(){
    const[ipt1, setipt1] = useState("");
    const[ipt2, setipt2] = useState("");
    const[ipt3, setipt3] = useState("");
    const[ipt4, setipt4] = useState("");
    const[ipt5, setipt5] = useState("");
    const[ipt6, setipt6] = useState("");

    const[kind, setKind] = useState("");


    const [smShow3, setSmShow3] = useState(false);
    const handleClose = () => setSmShow3(false);

    return(
        <>
            <div>
                {/* 퀴즈 드롭다운 */}
                <Dropdown>
                    <Dropdown.Toggle variant="success" id="dropdown-basic">
                        퀴즈 추가
                    </Dropdown.Toggle>
                    <Dropdown.Menu>
                        <Dropdown.Item onClick={() => {setKind("four"); setSmShow3(true);}}> 4지선다형 퀴즈 추가 </Dropdown.Item>
                    </Dropdown.Menu>
                </Dropdown>
            </div>
            <div>
            <Modal
                    size="sm"
                    show={smShow3}
                    onHide={() => setSmShow3(false)}
                    aria-labelledby="example-modal-sizes-title-sm"
                    >
                    <Modal.Header closeButton>
                        <Modal.Title id="example-modal-sizes-title-sm">
                        4지선다형 퀴즈 추가
                        </Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <Form.Group className='mb-3'>
                            <Form.Label>퀴즈!</Form.Label>
                            <Form.Control as="textarea" rows={3} value={ipt1} onChange={(e)=> setipt1(e.target.value)} placeholder="퀴즈 추가"/>
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>1번</Form.Label>
                            <Form.Control value={ipt2} onChange={(e)=> setipt2(e.target.value)} placeholder="1번 보기"/>
                            <Form.Label>2번</Form.Label>
                            <Form.Control value={ipt3} onChange={(e)=> setipt3(e.target.value)} placeholder="2번 보기"/>
                            <Form.Label>3번</Form.Label>
                            <Form.Control value={ipt4} onChange={(e)=> setipt4(e.target.value)} placeholder="3번 보기"/>
                            <Form.Label>4번</Form.Label>
                            <Form.Control value={ipt5} onChange={(e)=> setipt5(e.target.value)} placeholder="4번 보기"/>
                            <Form.Label>정답</Form.Label>
                            <Form.Control value={ipt6} onChange={(e)=> setipt6(e.target.value)} placeholder="정답번호"/>
                        </Form.Group>
                    </Modal.Body>
                    <Modal.Footer>
                    <Button type="button" className='btn btn-secondary' data-bs-dismiss="modal" onClick={handleClose}>Close</Button>
                        <Button className='btn btn-primary' onClick={(e)=>{
                            Axios.get("http://localhost:3000/api/metaquiz?add2="+ipt1+"&exp1="+ipt2 + "&exp2="+ipt3 + "&exp3="+ipt4 + "&exp4="+ipt5 + "&correct=" + ipt6 + "&Kind=" + kind).then(()=>{setSmShow3(false);});
                        setSmShow3(false);
                        }}>Save</Button>
                    </Modal.Footer>
                    </Modal>
            </div>
        </>
    );
}