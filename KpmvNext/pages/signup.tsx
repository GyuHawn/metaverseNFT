import {Form, Button, Row, Col, Container, Alert} from 'react-bootstrap';
import Axios from "axios";
import {useState} from "react";
import {useRouter} from "next/router";

const Signup = () => {
    const [userid, setuserid] = useState("");
    const [password, setpassword] = useState("");
    const [confirmpassword, setconfirmpassword] = useState("");
    const [username, setusername] = useState("");
    const [email, setemail] = useState("");
    const [text, settext] = useState("");

    const [onalert, setalert] = useState(false);
    const [useriderror, setuseriderror] = useState(false);
    const [passworderror, setpassworderror] = useState(false);
    const [confirmpassworderror, setconfirmpassworderror] = useState(false);
    const rt1 = useRouter();

    const onChangeUserId = (e: any) => {
        const userIdRegex = /^[A-Za-z0-9+]{5,15}$/;
        if ((!e.target.value || (userIdRegex.test(e.target.value)))) setuseriderror(false);
        else setuseriderror(true);
        setuserid(e.target.value);
    };

    const onChangePassword = (e: any) => {
        const passwordRegex = /^(?=.*[a-zA-z])(?=.*[0-9])(?=.*[$`~!@$!%*#^?&\\(\\)\-_=+]).{8,16}$/
        if ((!e.target.value || (passwordRegex.test(e.target.value)))) {
            setpassworderror(false);
        } else {
            setpassworderror(true);
        }

        if (!confirmpassword || e.target.value === confirmpassword) setconfirmpassworderror(false);
        else setconfirmpassworderror(true);
        setpassword(e.target.value);
    };
    const onChangeConfirmPassword = (e: any) => {
        if (password === e.target.value) setconfirmpassworderror(false);
        else setconfirmpassworderror(true);
        setconfirmpassword(e.target.value);
    };


    return (
        <div>
            <Container className="panel">
                <Alert key={'danger'} variant={'danger'} show={onalert}>
                    {text}
                </Alert>
                <Form>
                    <Form.Group as={Row} className="mb-3">
                        <Col sm>
                            <Form.Control maxLength={15} type="text" placeholder="UserID" value={userid}
                                          onChange={onChangeUserId}/>
                            {useriderror &&
                                <div className="text-bg-danger"> ID는 5자 이상 15자이하 영문자로 시작하는 문자 또는 숫자로 작성되어야 합니다.</div>}
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row} className="mb-3">
                        <Col sm>
                            <Form.Control maxLength={16} type="password" placeholder="Password" value={password}
                                          onChange={onChangePassword}/>
                            {passworderror &&
                                <div className={"text-bg-danger"}> 암호는 8 ~ 16자 영문, 숫자, 특수문자를 최소 한가지씩 조합 </div>}
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row} className="mb-3" controlId="formPlaintextPassword">
                        <Col sm>
                            <Form.Control maxLength={16} type="password" placeholder="Confirm Password"
                                          value={confirmpassword} onChange={onChangeConfirmPassword}/>
                            {confirmpassworderror && <div className={"text-bg-danger"}>암호가 일치하지 않습니다.</div>}
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row} className="mb-3">
                        <Col sm>
                            <Form.Control type="text" placeholder="Username" value={username} onChange={(e) => {
                                setusername(e.target.value)
                            }}/>
                        </Col>
                    </Form.Group>

                    <Form.Group as={Row} className="mb-3">
                        <Col sm>
                            <Form.Control type="email" placeholder="Email Address" value={email} onChange={(e) => {
                                setemail(e.target.value)
                            }}/>
                        </Col>
                    </Form.Group>
                    <br/>
                    <div className="d-grid gap-1">
                        <Button variant="secondary" onClick={() => {
                            Axios.get("http://localhost:3000/api/auth?add=" + userid + "&password=" + password + "&username=" + username + "&email=" + email + "&EOA=" + "&CA=" + "&wins=0")
                                .then(() => {
                                    alert("회원가입 성공");
                                    rt1.push('/login');
                                })
                                .catch((error) => {settext("이미 존재하는 아이디 입니다."); setalert(true)});

                        }}>
                            Sign Up
                        </Button>
                    </div>
                </Form>
            </Container>
        </div>
    );
}

export default Signup;