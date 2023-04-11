import useSWR from 'swr'
import Axios from "axios";
import {useState} from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import {Button, Table, Modal, Form, Stack} from 'react-bootstrap';
import Quiz3 from './quiz3';


const axios1 = (url: string) => Axios.get(url).then((res) => res.data);

const Quizlist2 = () => {
    const {data, error, isLoading} = useSWR("http://localhost:3000/api/metaquiz", axios1);
    const [smShow, setSmShow] = useState(false);
    const [ipt1, setipt1] = useState("");
    const [ipt2, setipt2] = useState("");
    const [ipt3, setipt3] = useState("");
    const [ipt4, setipt4] = useState("");
    const [ipt5, setipt5] = useState("");
    const [ipt6, setipt6] = useState("");
    const handleClose = () => setSmShow(false);

    if (error) {
        return <>error!</>;
    }
    if (!data) {
        return <>loading</>;
    }

    return (
        <>
            <Quiz3/>
            <Table striped bordered hover className={"mt-3"}>
                {data?.filter((e: { Kind: string }) => e.Kind === "it").map((e: { ITcontent: string, ITcorrect: String , ITexp1: String, ITexp2: String, ITexp3: String, ITexp4: String}) => {
                    return <>
                        <tr>
                            <th>퀴즈 내용:</th>
                            <th>{e.ITcontent}</th>
                            <th> 보기 :</th>
                            <th>{"1번:" + e.ITexp1 + ", 2번:"+ e.ITexp2+ ", 3번:" + e.ITexp3+ ", 4번:" + e.ITexp4}</th>
                            <th> 정답!! :</th>
                            <th>{e.ITcorrect}</th>
                            <tr>
                                <Stack direction="horizontal" gap={2}>
                                    <Button onClick={() => setSmShow(true)}>수정</Button>
                                    <Button onClick={() => {
                                        Axios.get("http://localhost:3000/api/metaquiz?del3=" + e.ITcontent);
                                        alert("삭제!!");
                                    }}>삭제</Button>
                                </Stack>
                            </tr>
                        </tr>
                    </>
                })
                }
            </Table>

            <Modal
                size="sm"
                show={smShow}
                onHide={() => setSmShow(false)}
                aria-labelledby="example-modal-sizes-title-sm"
            >
                <Modal.Header closeButton>
                    <Modal.Title id="example-modal-sizes-title-sm">
                        수정
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form.Group className='mb-3'>
                        <Form.Label>퀴즈</Form.Label>
                        <Form.Control as="textarea" rows={3} value={ipt1} onChange={(e) => setipt1(e.target.value)}
                                      placeholder="퀴즈 수정"/>
                    </Form.Group>
                    <Form.Group className='mb-3'>
                        <Form.Label> 1번 </Form.Label>
                        <Form.Control value={ipt2} onChange={(e) => setipt2(e.target.value)}
                                      placeholder="1번 보기"/>
                        <Form.Label> 2번 </Form.Label>
                        <Form.Control value={ipt3} onChange={(e) => setipt3(e.target.value)}
                                      placeholder="2번 보기"/>
                        <Form.Label> 3번 </Form.Label>
                        <Form.Control value={ipt4} onChange={(e) => setipt4(e.target.value)}
                                      placeholder="3번 보기"/>
                        <Form.Label> 4번 </Form.Label>
                        <Form.Control value={ipt5} onChange={(e) => setipt5(e.target.value)}
                                      placeholder="4번 보기"/>
                        <Form.Label> 정답 </Form.Label>
                        <Form.Control value={ipt6} onChange={(e) => setipt6(e.target.value)}
                                      placeholder="정답 번호"/>
                    </Form.Group>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleClose} variant="secondary">Close</Button>
                    <Button variant="primary" onClick={() => {
                        Axios.get("http://localhost:3000/api/metaquiz?update3=" + ipt1 + "&ITexp1="+ipt2 + "&ITexp2="+ipt3 + "&ITexp3="+ipt4 + "&ITexp4="+ipt5 + "&ITcorrect=" + ipt6).then(() => {
                            setSmShow(false);
                        });
                    }}>Save changes</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default Quizlist2;