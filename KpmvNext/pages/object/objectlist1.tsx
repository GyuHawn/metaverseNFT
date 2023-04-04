import {Accordion, Button, Form, Modal, Stack, Table} from "react-bootstrap";
import Axios from "axios";
import useSWR from "swr";
import {useState} from "react";
import Object from "@/pages/object/object";

const axios1 = (url: string) => Axios.get(url).then((res) => res.data);

const Objectlist =()=>{
    const {data, error, isLoading} = useSWR("/api/metabus", axios1);
    const [ipt1, setipt1] = useState("");
    const [ipt2, setipt2] = useState("");
    const [ipt3, setipt3] = useState("");
    const [smShow, setSmShow] = useState(false);
    const handleClose = () => setSmShow(false);

    return(
        <>
            <Object/>
            <Accordion className={"mt-3"}>{/*defaultActiveKey="0"*/}
                <Accordion.Item eventKey="0">
                    <Accordion.Header>구성요소1 list</Accordion.Header>
                    <Accordion.Body>
                        <div className='reservation_list'>
                            <h2> 구성요소 목록</h2>
                            <br/>
                            <Table striped bordered hover>
                                {data?.map((e: { name: string, x: Int32Array, y: Int32Array }) => {
                                    return <>
                                        <tr>
                                            <th>큐브 :</th>
                                            <td style={{width: "100px"}}><a href={'/user/' + e.name}>{e.name}</a>
                                            </td>
                                            <th> X좌표 :</th>
                                            <td>{e.x}</td>
                                            <th> Y좌표 :</th>
                                            <td>{e.y}</td>
                                            <td>
                                                <Stack direction="horizontal" gap={2}>
                                                    <Button onClick={() => {
                                                        setSmShow(true);
                                                        setipt1(e.name);
                                                    }}
                                                            className='btn btn-danger'>수정</Button>
                                                    <Button onClick={() => {
                                                        Axios.get("http://localhost:3000/api/metabus?del=" + e.name);
                                                        alert("삭제!!");
                                                    }} className='btn btn-danger'>삭제</Button>
                                                </Stack></td>
                                        </tr>
                                        <br/></>
                                })
                                }
                            </Table>
                        </div>
                    </Accordion.Body>
                </Accordion.Item>
            </Accordion>

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
                        <Form.Label>박스</Form.Label>
                        <Form.Control value={ipt1} onChange={(e) => setipt1(e.target.value)} placeholder="박스 이름"/>
                    </Form.Group>
                    <Form.Group className='mb-3'>
                        <Form.Label>x좌표</Form.Label>
                        <Form.Control value={ipt2} onChange={(e) => setipt2(e.target.value)} placeholder="x 좌표"/>
                    </Form.Group>
                    <Form.Group className='mb-3'>
                        <Form.Label>y좌표</Form.Label>
                        <Form.Control value={ipt3} onChange={(e) => setipt3(e.target.value)} placeholder="y 좌표"/>
                    </Form.Group>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleClose} variant="secondary">Close</Button>
                    <Button variant="primary" onClick={() => {
                        Axios.get("http://localhost:3000/api/metabus?update=" + ipt1 + "&PosX=" + ipt2 + "&PosY=" + ipt3).then(() => {
                            setSmShow(false);
                        });
                    }}>Save changes</Button>
                </Modal.Footer>
            </Modal>

        </>
    )
}

export default Objectlist;