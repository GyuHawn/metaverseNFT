import {Button, Dropdown, Form, Modal} from "react-bootstrap";
import Axios from "axios";
import {useState} from "react";

const Object = () =>{
    const [ipt1, setipt1] = useState("");
    const [ipt2, setipt2] = useState("");
    const [ipt3, setipt3] = useState("");
    const [smShow, setSmShow] = useState(false);
    const handleClose = () => setSmShow(false);

    return(
        <>
            <div>
                <Dropdown>
                    <Dropdown.Toggle variant="success" id="dropdown-basic">
                        Object 추가
                    </Dropdown.Toggle>
                    <Dropdown.Menu>
                        <Dropdown.Item onClick={() => setSmShow(true)}> 구성요소1 </Dropdown.Item>
                    </Dropdown.Menu>
                </Dropdown>
            </div>

            <Modal
                size="sm"
                show={smShow}
                onHide={() => setSmShow(false)}
                aria-labelledby="example-modal-sizes-title-sm"
            >
                <Modal.Header closeButton>
                    <Modal.Title id="example-modal-sizes-title-sm">
                        구성요소1
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
                <div className='modal-footer'>
                    <Button type="button" className='btn btn-secondary' data-bs-dismiss="modal"
                            onClick={handleClose}>Close</Button>
                    <Button className='btn btn-primary' onClick={(e) => {
                        Axios.get("/api/metabus?add=" + ipt1 + "&PosX=" + ipt2 + "&PosY=" + ipt3);
                        history.back();
                    }}>Save</Button>
                </div>
            </Modal>
        </>
    )
}

export default Object;