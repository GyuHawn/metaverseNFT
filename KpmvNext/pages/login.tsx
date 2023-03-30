import {Form, Button, Row, Col, Container, Alert} from "react-bootstrap";
import *as Session from "@/pages/api/session";
import {useRouter} from "next/router";
import {useState} from "react";
import Axios from "axios";
import useSWR from "swr";
import Link from "next/link";

const axios1 = (url: string) => Axios.get(url).then((res) => res.data);

function Login() {
    const LoginUrl = '/api/session?login';
    const DataUrl = '/api/session?data';

    const {data, error, isLoading} = useSWR(DataUrl, axios1);
    const rt1 = useRouter();
    const [iptuid, setipuid] = useState("");
    const [iptpwd, setiptpwd] = useState("");
    const [text, settext] = useState("");
    const [alert, setalert] = useState(false);

    if(data?.logon==true){
        rt1.push('/');
    }
    if (error) {
        return (<>error:{JSON.stringify(error)}</>);
    }
    return (
        <div>
            <Container className="panel">
                <Alert key={'danger'} variant={'danger'} show={alert}>
                    {text}
                </Alert>
                <Form>
                    <Form.Group as={Row} className="mb-3">
                        <Col sm>
                            <Form.Control type="text" placeholder="UserID" value={iptuid} onChange={(e) => setipuid(e.target.value)}/>
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row} className="mb-3">
                        <Col sm>
                            <Form.Control type="password" placeholder="Password" value = {iptpwd} onChange={(e) => setiptpwd(e.target.value)}/>
                        </Col>
                    </Form.Group>
                    <br/>
                    <div className="d-grid gap-1">
                        <Button variant="secondary" onClick={() => {
                            Axios.post(LoginUrl, {Userid: iptuid, Userpwd: iptpwd}).then(() => {
                                rt1.push('/');
                                rt1.reload();
                            }).catch((error) => {settext("아이디나 비밀번호가 틀렸습니다!!"); setalert(true)});
                        }}>
                            Sign In
                        </Button>
                    </div>
                </Form>
                <span className="text">New to account? <Link href="/signup" className="link">Sign Up</Link></span>
            </Container>
        </div>
    );
}

export default Login